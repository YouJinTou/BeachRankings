namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Enums;
    using BeachRankings.Models.Interfaces;
    using System.Data.Entity;
    using System.Collections.Generic;

    public class RegionRepository : GenericRepository<Region>, IRegionRepository
    {
        private DbContext dbContext;
        private IDbSet<Region> entitySet;

        public RegionRepository(DbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = dbContext.Set<Region>();
        }

        public IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix)
        {
            LuceneSearch.Index = Index.RegionIndex;
            var results = LuceneSearch.SearchByPrefix(prefix, 10);

            return results;
        }

        public void AddRegionToIndex(Region region)
        {
            LuceneSearch.Index = Index.RegionIndex;

            LuceneSearch.AddUpdateIndexEntry(region);
        }
    }
}