namespace BeachRankings.Data.Services.Search
{
    using BeachRankings.Models.Interfaces;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Store;
    using Version = Lucene.Net.Util.Version;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal static class LuceneSearch
    {
        private static readonly string beachIndexDirPath;

        private static FSDirectory beachIndexDir;
        
        static LuceneSearch()
        {
            beachIndexDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "BeachIndex");
        }

        private static FSDirectory BeachIndexDir
        {
            get
            {
                if (beachIndexDir == null)
                {
                    beachIndexDir = FSDirectory.Open(new DirectoryInfo(beachIndexDirPath));
                }

                if (IndexWriter.IsLocked(beachIndexDir))
                {
                    IndexWriter.Unlock(beachIndexDir);
                }

                var lockFilePath = Path.Combine(beachIndexDirPath, "write.lock");

                if (File.Exists(lockFilePath))
                {
                    File.Delete(lockFilePath);
                }

                return beachIndexDir;
            }
        }

        public static IEnumerable<int> Search(string searchQuery, string searchField = null)
        {
            var formattedSearchQuery = searchQuery.Replace("*", string.Empty).Replace("?", string.Empty);

            if (string.IsNullOrEmpty(formattedSearchQuery))
            {
                return new List<int>();
            }

            using (var searcher = new IndexSearcher(BeachIndexDir, true))
            {
                var hitsLimit = 1000;
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                    var query = ParseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, hitsLimit).ScoreDocs;
                    var results = GetDocumentIds(hits, searcher);

                    analyzer.Close();
                    searcher.Dispose();

                    return results;
                }
                else {
                    var searchFields = new string[] { "Name", "Location", "Description", "WaterBody", "ApproximateAddress", "Coordinates" };
                    var parser = new MultiFieldQueryParser(Version.LUCENE_30, searchFields, analyzer);
                    var query = ParseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, null, hitsLimit, Sort.RELEVANCE).ScoreDocs;
                    var results = GetDocumentIds(hits, searcher);

                    analyzer.Close();
                    searcher.Dispose();

                    return results;
                }
            }
        }
        
        public static void AddBeachIndexEntry(IBeachSearchable beach)
        {
            AddBeachIndexEntries(new List<IBeachSearchable>() { beach });
        }

        public static void AddBeachIndexEntries(IEnumerable<IBeachSearchable> beaches)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);

            using (var writer = new IndexWriter(BeachIndexDir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var beach in beaches)
                {
                    AddDocument(beach, writer);
                }

                analyzer.Close();
                writer.Dispose();
            }
        }

        public static void DeleteBeachIndexEntry(int id)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);

            using (var writer = new IndexWriter(BeachIndexDir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var oldDoc = new TermQuery(new Term("Id", id.ToString()));

                writer.DeleteDocuments(oldDoc);

                analyzer.Close();
                writer.Dispose();
            }
        }

        public static bool ClearBeachIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                using (var writer = new IndexWriter(BeachIndexDir, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
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

            using (var writer = new IndexWriter(BeachIndexDir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
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
        
        private static void AddDocument(IBeachSearchable beach, IndexWriter writer)
        {
            var oldDoc = new TermQuery(new Term("Id", beach.Id.ToString()));

            writer.DeleteDocuments(oldDoc);

            var newDoc = new Document();

            newDoc.Add(new Field("Id", beach.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            newDoc.Add(new Field("Name", beach.Name, Field.Store.YES, Field.Index.ANALYZED));
            newDoc.Add(new Field("Location", beach.Location, Field.Store.YES, Field.Index.ANALYZED));
            newDoc.Add(new Field("Description", beach.Description ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            newDoc.Add(new Field("WaterBody", beach.WaterBody ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            newDoc.Add(new Field("ApproximateAddress", beach.ApproximateAddress ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            newDoc.Add(new Field("Coordinates", beach.Coordinates ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));

            writer.AddDocument(newDoc);
        }

        private static int GetDocumentId(Document doc)
        {
            return int.Parse(doc.Get("Id"));
        }

        private static IEnumerable<int> GetDocumentIds(IEnumerable<Document> docs)
        {
            return docs.Select(GetDocumentId).ToList();
        }

        private static IEnumerable<int> GetDocumentIds(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => GetDocumentId(searcher.Doc(hit.Doc))).ToList();
        }
    }
}