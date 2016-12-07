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
            var searchService = new LuceneSearch(Index.WaterBodyIndex);
            var searchables = searchService.SearchByPrefix(prefix, 10);
            var results = new List<WaterBodySearchResultModel>();

            foreach (var searchable in searchables)
            {
                results.Add((WaterBodySearchResultModel)searchable);
            }

            return results.Where(wb => wb.BeachCount > 0).OrderByDescending(r => r.BeachCount);
        }

        public void AddUpdateIndexEntry(ISearchable waterBody)
        {
            var searchService = new LuceneSearch(Index.WaterBodyIndex);

            searchService.AddUpdateIndexEntry(waterBody);
        }

        public void DeleteIndexEntry(ISearchable waterBody)
        {
            var searchService = new LuceneSearch(Index.WaterBodyIndex);

            searchService.DeleteIndexEntry(waterBody);
        }
    }
}