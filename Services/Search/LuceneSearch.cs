namespace BeachRankings.Services.Search
{
    using BeachRankings.Models.Interfaces;
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.Search;
    using Lucene.Net.Store;
    using Lucene.Net.QueryParsers;
    using Version = Lucene.Net.Util.Version;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    
    public static class LuceneSearch
    {
        private static string indexDirPath;
        private static FSDirectory indexDir;
        private static Indices index;

        public static Indices Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;

                switch (index)
                {
                    case Indices.BeachIndex:
                        indexDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "BeachIndex");

                        break;
                    case Indices.LocationIndex:
                        indexDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "LocationIndex");

                        break;
                    default:
                        indexDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "LocationIndex");
                        index = Indices.LocationIndex;

                        break;
                }

                System.IO.Directory.CreateDirectory(indexDirPath);
            }
        }

        private static FSDirectory IndexDir
        {
            get
            {
                if (indexDir == null)
                {
                    indexDir = FSDirectory.Open(new DirectoryInfo(indexDirPath));
                }

                if (IndexWriter.IsLocked(indexDir))
                {
                    IndexWriter.Unlock(indexDir);
                }

                var lockFilePath = Path.Combine(indexDirPath, "write.lock");

                if (File.Exists(lockFilePath))
                {
                    File.Delete(lockFilePath);
                }

                return indexDir;
            }
        }

        private static IEnumerable<string> SearchFields
        {
            get
            {
                switch (Index)
                {
                    case Indices.LocationIndex:
                        return new string[] { "Name" };
                    case Indices.BeachIndex:
                        return new string[] { "Name", "Description", "WaterBody", "ApproximateAddress" };
                    default:
                        return new string[] { "Name" };
                }                
            }
        }

        public static IEnumerable<int> Search(string searchQuery, string searchField = null)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return null;
            }

            using (var searcher = new IndexSearcher(IndexDir, true))
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                var hitsLimit = 1000;
                IEnumerable<ScoreDoc> hits;
                IEnumerable<int> results;

                if (string.IsNullOrEmpty(searchField))
                {
                    var queries = GetFuzzyQueries(searchQuery);
                    hits = GetHits(queries, searcher, hitsLimit);
                }
                else
                {
                    var query = new FuzzyQuery(new Term(searchField, searchQuery));
                    hits = searcher.Search(query, hitsLimit).ScoreDocs;
                }

                results = GetDocumentIds(hits, searcher);

                analyzer.Close();
                searcher.Dispose();

                return results;
            }
        }

        public static IEnumerable<int> SearchByPrefix(string prefix, int maxItems)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return null;
            }

            var parser = new MultiFieldQueryParser(Version.LUCENE_30, (string[])SearchFields, new KeywordAnalyzer())
            {
                DefaultOperator = QueryParser.Operator.OR
            };
            var query = ParseQuery(prefix + "*", parser);

            using (var searcher = new IndexSearcher(IndexDir, true))
            {
                var hits = searcher.Search(query, maxItems).ScoreDocs;
                var resultIds = GetDocumentIds(hits, searcher);

                return resultIds;
            }
        }

        public static void AddUpdateIndexEntry(ISearchable searchable)
        {
            AddUpdateIndexEntries(new List<ISearchable>() { searchable });
        }

        public static void AddUpdateIndexEntries(IEnumerable<ISearchable> searchables)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);

            using (var writer = new IndexWriter(IndexDir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var searchable in searchables)
                {
                    LuceneEntryCreator.AddUpdateDocument(searchable, writer);
                }

                analyzer.Close();
                writer.Dispose();
            }
        }

        public static void DeleteIndexEntry(int id)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);

            using (var writer = new IndexWriter(IndexDir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var oldDoc = new TermQuery(new Term("Id", id.ToString()));

                writer.DeleteDocuments(oldDoc);

                analyzer.Close();
                writer.Dispose();
            }
        }

        public static bool ClearIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                using (var writer = new IndexWriter(IndexDir, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.DeleteAll();

                    analyzer.Close();
                    writer.Dispose();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static void OptimizeSearch()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);

            using (var writer = new IndexWriter(IndexDir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();

                writer.Optimize();

                writer.Dispose();
            }
        }

        private static Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;

            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }

            return query;
        }

        private static IEnumerable<FuzzyQuery> GetFuzzyQueries(string searchQuery)
        {
            var fuzzyQueries = new List<FuzzyQuery>();
            var queryTokens = searchQuery.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var searchField in SearchFields)
            {
                foreach (var token in queryTokens)
                {
                    var fuzzyQuery = new FuzzyQuery(new Term(searchField, token), 0.4f);

                    fuzzyQueries.Add(fuzzyQuery);
                }
            }

            return fuzzyQueries;
        }

        private static IEnumerable<ScoreDoc> GetHits(IEnumerable<FuzzyQuery> queries, IndexSearcher searcher, int hitsLimit)
        {
            var hits = new List<ScoreDoc>();

            foreach (var fuzzyQuery in queries)
            {
                hits.AddRange(searcher.Search(fuzzyQuery, null, hitsLimit, Sort.RELEVANCE).ScoreDocs);
            }

            return hits;
        }
        
        private static int GetDocumentId(Document doc)
        {
            return int.Parse(doc.Get("Id"));            
        }

        private static IEnumerable<int> GetDocumentIds(IEnumerable<Document> docs)
        {
            return docs.Select(GetDocumentId).Distinct().ToList();
        }

        private static IEnumerable<int> GetDocumentIds(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => GetDocumentId(searcher.Doc(hit.Doc))).Distinct().ToList();
        }        
    }
}