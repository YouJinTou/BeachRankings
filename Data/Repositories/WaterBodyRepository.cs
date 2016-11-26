namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Data.Repositories.Interfaces;
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Enums;
    using BeachRankings.Services.Search.Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class WaterBodyRepository : GenericRepository<WaterBody>, IWaterBodyRepository
    {
        private DbContext dbContext;
        private IDbSet<WaterBody> entitySet;

        public WaterBodyRepository(DbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = dbContext.Set<WaterBody>();
        }

        public IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix)
        {
            LuceneSearch.Index = Index.WaterBodyIndex;
            var searchables = LuceneSearch.SearchByPrefix(prefix, 10);
            var results = new List<PlaceSearchResultModel>();

            foreach (var searchable in searchables)
            {
                results.Add((PlaceSearchResultModel)searchable);
            }

            return results.Where(r => r.BeachCount > 0).OrderByDescending(r => r.BeachCount);
        }

        public void AddUpdateIndexEntry(ISearchable waterBody)
        {
            LuceneSearch.Index = Index.WaterBodyIndex;

            LuceneSearch.AddUpdateIndexEntry(waterBody);
        }

        public void DeleteIndexEntry(ISearchable waterBody)
        {
            LuceneSearch.Index = Index.WaterBodyIndex;

            LuceneSearch.DeleteIndexEntry(waterBody);
        }
    }
}