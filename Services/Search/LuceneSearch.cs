namespace BeachRankings.Services.Search
{
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Search.Enums;
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
        private static FSDirectory beachIndexDir;
        private static FSDirectory countryIndexDir;
        private static FSDirectory primaryDivisionIndexDir;
        private static FSDirectory secondaryDivisionIndexDir;
        private static FSDirectory tertiaryDivisionIndexDir;
        private static FSDirectory quaternaryDivisionIndexDir;
        private static FSDirectory waterBodyIndexDir;
        
        private static Index index;
        private static ModelType modelType;

        static LuceneSearch()
        {
            if (indexDir != null)
            {
                return;
            }

            var beachIndexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "BeachIndex");
            var countryIndexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "CountryIndex");
            var primaryDivisionIndexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "PrimaryDivisionIndex");
            var secondaryDivisionIndexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "SecondaryDivisionIndex");
            var tertiaryDivisionIndexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "TertiaryDivisionIndex");
            var quaternaryDivisionIndexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "QuaternaryDivisionIndex");
            var waterBodyIndexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "WaterBodyIndex");

            System.IO.Directory.CreateDirectory(beachIndexPath);
            System.IO.Directory.CreateDirectory(countryIndexPath);
            System.IO.Directory.CreateDirectory(primaryDivisionIndexPath);
            System.IO.Directory.CreateDirectory(secondaryDivisionIndexPath);
            System.IO.Directory.CreateDirectory(tertiaryDivisionIndexPath);
            System.IO.Directory.CreateDirectory(quaternaryDivisionIndexPath);
            System.IO.Directory.CreateDirectory(waterBodyIndexPath);

            beachIndexDir = FSDirectory.Open(new DirectoryInfo(beachIndexPath));
            countryIndexDir = FSDirectory.Open(new DirectoryInfo(countryIndexPath));
            primaryDivisionIndexDir = FSDirectory.Open(new DirectoryInfo(primaryDivisionIndexPath));
            secondaryDivisionIndexDir = FSDirectory.Open(new DirectoryInfo(secondaryDivisionIndexPath));
            tertiaryDivisionIndexDir = FSDirectory.Open(new DirectoryInfo(tertiaryDivisionIndexPath));
            quaternaryDivisionIndexDir = FSDirectory.Open(new DirectoryInfo(quaternaryDivisionIndexPath));
            waterBodyIndexDir = FSDirectory.Open(new DirectoryInfo(waterBodyIndexPath));
        }

        public static Index Index
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
                    case Index.BeachIndex:
                        ModelType = ModelType.Beach;
                        IndexDir = beachIndexDir;
                        indexDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "BeachIndex");

                        break;
                    case Index.CountryIndex:
                        ModelType = ModelType.Place;
                        IndexDir = countryIndexDir;
                        indexDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "CountryIndex");

                        break;
                    case Index.PrimaryDivisionIndex:
                        ModelType = ModelType.Place;
                        IndexDir = primaryDivisionIndexDir;
                        indexDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "PrimaryDivisionIndex");

                        break;
                    case Index.SecondaryDivisionIndex:
                        ModelType = ModelType.Place;
                        IndexDir = secondaryDivisionIndexDir;
                        indexDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "SecondaryDivisionIndex");

                        break;
                    case Index.TertiaryDivisionIndex:
                        ModelType = ModelType.Place;
                        IndexDir = tertiaryDivisionIndexDir;
                        indexDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "TertiaryDivisionIndex");

                        break;
                    case Index.QuaternaryDivisionIndex:
                        ModelType = ModelType.Place;
                        IndexDir = quaternaryDivisionIndexDir;
                        indexDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "QuaternaryDivisionIndex");

                        break;
                    case Index.WaterBodyIndex:
                        ModelType = ModelType.Place;
                        IndexDir = waterBodyIndexDir;
                        indexDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "WaterBodyIndex");

                        break;                    
                    default:
                        index = Index.PrimaryDivisionIndex;
                        ModelType = ModelType.Place;
                        IndexDir = primaryDivisionIndexDir;
                        indexDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "PrimaryDivisionIndex");

                        break;
                }
            }
        }

        internal static ModelType ModelType
        {
            get
            {
                return modelType;
            }
            private set
            {
                modelType = value;
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
            set
            {
                indexDir = value;
            }
        }

        private static IEnumerable<string> SearchFields
        {
            get
            {
                switch (Index)
                {
                    case Index.BeachIndex:
                        return new string[] { "Name", "WaterBodyName", "Description", "ApproximateAddress" };
                    case Index.PrimaryDivisionIndex:
                    case Index.SecondaryDivisionIndex:
                    case Index.TertiaryDivisionIndex:
                    case Index.QuaternaryDivisionIndex:
                    case Index.WaterBodyIndex:
                    case Index.CountryIndex:
                        return new string[] { "Name" };                    
                    default:
                        return new string[] { "Name" };
                }                
            }
        }

        public static IEnumerable<ISearchable> Search(string searchQuery, string searchField = null)
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
                IEnumerable<ISearchable> results;

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

                results = MapDocsToModels(hits, searcher);

                analyzer.Close();
                searcher.Dispose();

                return results;
            }
        }

        public static IEnumerable<ISearchable> SearchByPrefix(string prefix, int maxItems)
        {
            var dirEmpty = !System.IO.Directory.EnumerateFileSystemEntries(indexDirPath).Any();

            if (dirEmpty)
            {
                return new List<ISearchable>();
            }

            var parser = new MultiFieldQueryParser(Version.LUCENE_30, (string[])SearchFields, new KeywordAnalyzer())
            {
                DefaultOperator = QueryParser.Operator.OR
            };
            var query = ParseQuery(prefix + "*", parser);
            var filter = new QueryWrapperFilter(query);
            var sort = new Sort(new SortField("BeachCount", SortField.INT, true));

            using (var searcher = new IndexSearcher(IndexDir, true))
            {
                var hits = searcher.Search(query, filter, maxItems, sort).ScoreDocs;
                var results = MapDocsToModels(hits, searcher);

                return results;
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
                    LuceneEntryFactory.AddUpdateDocument(searchable, writer);
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

                // TO-DO UPDATE REFERENCES

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
        
        private static ISearchable MapDocToModel(Document doc)
        {
            return LuceneModelFactory.MapDocToModel(doc);            
        }

        private static IEnumerable<ISearchable> MapDocsToModels(IEnumerable<Document> docs)
        {
            return docs.Select(MapDocToModel).Distinct().ToList();
        }

        private static IEnumerable<ISearchable> MapDocsToModels(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => MapDocToModel(searcher.Doc(hit.Doc))).Distinct().ToList();
        }

        private static void TryCreateIndex(FSDirectory index, string path)
        {
            if (index == null)
            {
                index = FSDirectory.Open(path);
            }
        }
    }
}