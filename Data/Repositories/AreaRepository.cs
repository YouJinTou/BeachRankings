namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Enums;
    using Models.Interfaces;
    using System.Collections.Generic;
    using System.Data.Entity;

    public class AreaRepository : GenericRepository<Area>, IAreaRepository
    {
        private DbContext dbContext;
        private IDbSet<Area> entitySet;

        public AreaRepository(DbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = dbContext.Set<Area>();
        }

        public IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix)
        {
            LuceneSearch.Index = Index.AreaIndex;
            var results = LuceneSearch.SearchByPrefix(prefix, 10);

            return results;
        }

        public void AddAreaToIndex(Area area)
        {
            LuceneSearch.Index = Index.RegionIndex;

            LuceneSearch.AddUpdateIndexEntry(area);
        }
    }
}