namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Enums;
    using BeachRankings.Services.Search.Models;
    using BeachRankings.Data.Repositories.Interfaces;
    using System.Collections.Generic;
    using System.Data.Entity;

    public class BeachRepository : GenericRepository<Beach>, IBeachRepository
    {
        private DbContext dbContext;
        private IDbSet<Beach> entitySet;

        public BeachRepository(DbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = dbContext.Set<Beach>();
        }

        public IEnumerable<BeachSearchResultModel> GetSearchResultsByKeyStroke(string prefix)
        {
            LuceneSearch.Index = Index.BeachIndex;
            var searchables = LuceneSearch.SearchByPrefix(prefix, 10);
            var results = new List<BeachSearchResultModel>();

            foreach (var searchble in searchables)
            {
                results.Add((BeachSearchResultModel)searchble);
            }

            return results;
        }

        public void AddBeachToIndex(Beach beach)
        {
            LuceneSearch.Index = Index.BeachIndex;

            LuceneSearch.AddUpdateIndexEntry(beach);
        }
    }
}