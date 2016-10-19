namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Enums;
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

        public void AddWaterBodyToIndex(WaterBody waterBody)
        {
            LuceneSearch.Index = Index.WaterBodyIndex;

            LuceneSearch.AddUpdateIndexEntry(waterBody);
        }
    }
}