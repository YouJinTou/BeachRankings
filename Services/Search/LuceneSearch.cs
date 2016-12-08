namespace BeachRankings.Services.Search
{
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Search.Enums;
    using BeachRankings.Services.Formatters;
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

    public class LuceneSearch
    {
        private readonly string AppDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");

        private string indexDirPath;
        private FSDirectory indexDir;        
        private Index index;
        private ModelType modelType;

        public LuceneSearch(Index index)
        {
            this.Index = index;

            var appDataDirs = new DirectoryInfo(this.AppDataPath).GetDirectories();
            var alreadyInitialized = appDataDirs.Any(d => d.Name.Contains("Index"));

            if (!alreadyInitialized)
            {
                this.CreateIndexDirectories();
            }
        }

        public Index Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;

                switch (this.index)
                {
                    case Index.BeachIndex:
                        this.modelType = ModelType.Beach;
                        this.indexDirPath = Path.Combine(this.AppDataPath, "BeachIndex");

                        break;
                    case Index.ContinentIndex:
                        this.modelType = ModelType.Continent;
                        this.indexDirPath = Path.Combine(this.AppDataPath, "ContinentIndex");

                        break;
                    case Index.CountryIndex:
                        this.modelType = ModelType.Country;
                        this.indexDirPath = Path.Combine(this.AppDataPath, "CountryIndex");

                        break;
                    case Index.PrimaryDivisionIndex:
                        this.modelType = ModelType.PrimaryDivision;
                        this.indexDirPath = Path.Combine(this.AppDataPath, "PrimaryDivisionIndex");

                        break;
                    case Index.SecondaryDivisionIndex:
                        this.modelType = ModelType.SecondaryDivision;
                        this.indexDirPath = Path.Combine(this.AppDataPath, "SecondaryDivisionIndex");

                        break;
                    case Index.TertiaryDivisionIndex:
                        this.modelType = ModelType.TertiaryDivision;
                        this.indexDirPath = Path.Combine(this.AppDataPath, "TertiaryDivisionIndex");

                        break;
                    case Index.QuaternaryDivisionIndex:
                        this.modelType = ModelType.QuaternaryDivision;
                        this.indexDirPath = Path.Combine(this.AppDataPath, "QuaternaryDivisionIndex");

                        break;
                    case Index.WaterBodyIndex:
                        this.modelType = ModelType.WaterBody;
                        this.indexDirPath = Path.Combine(this.AppDataPath, "WaterBodyIndex");

                        break;
                    default:
                        this.index = Index.PrimaryDivisionIndex;
                        this.modelType = ModelType.PrimaryDivision;
                        this.indexDirPath = Path.Combine(this.AppDataPath, "PrimaryDivisionIndex");

                        break;
                }

                this.IndexDir = FSDirectory.Open(new DirectoryInfo(this.indexDirPath));
            }
        }

        private FSDirectory IndexDir
        {
            get
            {
                if (this.indexDir == null)
                {
                    this.indexDir = FSDirectory.Open(new DirectoryInfo(this.indexDirPath));
                }

                if (IndexWriter.IsLocked(this.indexDir))
                {
                    IndexWriter.Unlock(this.indexDir);
                }

                var lockFilePath = Path.Combine(this.indexDirPath, "write.lock");

                if (File.Exists(lockFilePath))
                {
                    File.Delete(lockFilePath);
                }

                return this.indexDir;
            }
            set
            {
                this.indexDir = value;
            }
        }

        private IEnumerable<string> SearchFields
        {
            get
            {
                switch (this.Index)
                {
                    case Index.BeachIndex:
                        return new string[] { "Name", "Description", "Address" };
                    case Index.PrimaryDivisionIndex:
                    case Index.SecondaryDivisionIndex:
                    case Index.TertiaryDivisionIndex:
                    case Index.QuaternaryDivisionIndex:
                    case Index.WaterBodyIndex:
                    case Index.CountryIndex:
                    case Index.ContinentIndex:
                        return new string[] { "Name" };
                    default:
                        return new string[] { "Name" };
                }
            }
        }

        public IEnumerable<ISearchable> Search(string searchQuery, string searchField = null)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return null;
            }

            using (var searcher = new IndexSearcher(this.IndexDir, true))
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                var hitsLimit = 1000;
                var formatter = new NameFormatter();
                var formattedSearchQuery = formatter.GetFormattedPlaceName(searchQuery);
                IEnumerable<ScoreDoc> hits;
                IEnumerable<ISearchable> results;

                if (string.IsNullOrEmpty(searchField))
                {
                    var queries = this.GetFuzzyQueries(formattedSearchQuery);
                    hits = this.GetHits(queries, searcher, hitsLimit);
                }
                else
                {
                    var query = new FuzzyQuery(new Term(searchField, formattedSearchQuery));
                    hits = searcher.Search(query, hitsLimit).ScoreDocs;
                }

                results = this.MapDocsToModels(hits, searcher);

                analyzer.Close();
                searcher.Dispose();

                return results;
            }
        }

        public IEnumerable<ISearchable> SearchByPrefix(string prefix, int maxItems)
        {
            var dirEmpty = !System.IO.Directory.EnumerateFileSystemEntries(this.indexDirPath).Any();

            if (dirEmpty)
            {
                return new List<ISearchable>();
            }

            var parser = new MultiFieldQueryParser(Version.LUCENE_30, (string[])this.SearchFields, new KeywordAnalyzer())
            {
                DefaultOperator = QueryParser.Operator.OR
            };
            var formatter = new NameFormatter();
            var formattedPrefix = formatter.GetFormattedPlaceName(prefix);
            var query = this.ParseQuery(formattedPrefix + "*", parser);
            var filter = new QueryWrapperFilter(query);
            var sort = new Sort(new SortField("BeachCount", SortField.INT, true));

            using (var searcher = new IndexSearcher(this.IndexDir, true))
            {
                var hits = searcher.Search(query, filter, maxItems, sort).ScoreDocs;
                var results = this.MapDocsToModels(hits, searcher);

                return results;
            }
        }

        public void AddUpdateIndexEntry(ISearchable searchable)
        {
            this.AddUpdateIndexEntries(new List<ISearchable>() { searchable });
        }

        public void AddUpdateIndexEntries(IEnumerable<ISearchable> searchables)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);

            using (var writer = new IndexWriter(this.IndexDir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var entryFactory = new LuceneEntryFactory(this.modelType, writer);

                foreach (var searchable in searchables)
                {
                    entryFactory.AddUpdateDocument(searchable);
                }

                analyzer.Close();
                writer.Dispose();
            }
        }

        public void DeleteIndexEntry(ISearchable searchable)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);

            using (var writer = new IndexWriter(this.IndexDir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var oldDoc = new TermQuery(new Term("Id", searchable.Id.ToString()));

                writer.DeleteDocuments(oldDoc);

                analyzer.Close();
                writer.Dispose();
            }
        }

        public bool ClearIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                using (var writer = new IndexWriter(this.IndexDir, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
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

        public void OptimizeSearch()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);

            using (var writer = new IndexWriter(this.IndexDir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();

                writer.Optimize();

                writer.Dispose();
            }
        }

        private Query ParseQuery(string searchQuery, QueryParser parser)
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

        private IEnumerable<FuzzyQuery> GetFuzzyQueries(string searchQuery)
        {
            var fuzzyQueries = new List<FuzzyQuery>();
            var queryTokens = searchQuery.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var searchField in this.SearchFields)
            {
                foreach (var token in queryTokens)
                {
                    var fuzzyQuery = new FuzzyQuery(new Term(searchField, token), 0.4f);

                    fuzzyQueries.Add(fuzzyQuery);
                }
            }

            return fuzzyQueries;
        }

        private IEnumerable<ScoreDoc> GetHits(IEnumerable<FuzzyQuery> queries, IndexSearcher searcher, int hitsLimit)
        {
            var hits = new List<ScoreDoc>();

            foreach (var fuzzyQuery in queries)
            {
                hits.AddRange(searcher.Search(fuzzyQuery, null, hitsLimit, Sort.RELEVANCE).ScoreDocs);
            }

            return hits;
        }

        private ISearchable MapDocToModel(Document doc)
        {
            var modelFactory = new LuceneModelFactory(this.modelType);

            return modelFactory.MapDocToModel(doc);
        }

        private IEnumerable<ISearchable> MapDocsToModels(IEnumerable<Document> docs)
        {
            return docs.Select(this.MapDocToModel).Distinct().ToList();
        }

        private IEnumerable<ISearchable> MapDocsToModels(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => this.MapDocToModel(searcher.Doc(hit.Doc))).Distinct().ToList();
        }

        private void CreateIndexDirectories()
        {
            var beachIndexPath = Path.Combine(this.AppDataPath, "BeachIndex");
            var continentIndexPath = Path.Combine(this.AppDataPath, "ContinentIndex");
            var countryIndexPath = Path.Combine(this.AppDataPath, "CountryIndex");
            var primaryDivisionIndexPath = Path.Combine(this.AppDataPath, "PrimaryDivisionIndex");
            var secondaryDivisionIndexPath = Path.Combine(this.AppDataPath, "SecondaryDivisionIndex");
            var tertiaryDivisionIndexPath = Path.Combine(this.AppDataPath, "TertiaryDivisionIndex");
            var quaternaryDivisionIndexPath = Path.Combine(this.AppDataPath, "QuaternaryDivisionIndex");
            var waterBodyIndexPath = Path.Combine(this.AppDataPath, "WaterBodyIndex");

            System.IO.Directory.CreateDirectory(beachIndexPath);
            System.IO.Directory.CreateDirectory(continentIndexPath);
            System.IO.Directory.CreateDirectory(countryIndexPath);
            System.IO.Directory.CreateDirectory(primaryDivisionIndexPath);
            System.IO.Directory.CreateDirectory(secondaryDivisionIndexPath);
            System.IO.Directory.CreateDirectory(tertiaryDivisionIndexPath);
            System.IO.Directory.CreateDirectory(quaternaryDivisionIndexPath);
            System.IO.Directory.CreateDirectory(waterBodyIndexPath);
        }
    }
}