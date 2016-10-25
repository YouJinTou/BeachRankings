namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Data.Repositories.Interfaces;
    using BeachRankings.Models;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Enums;
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;
    using System.Data.Entity;

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
            var results = LuceneSearch.SearchByPrefix(prefix, 10);

            return results;
        }

        public void AddWaterBodyToIndex(WaterBody waterBody)
        {
            LuceneSearch.Index = Index.WaterBodyIndex;

            LuceneSearch.AddUpdateIndexEntry(waterBody);
        }
    }
}