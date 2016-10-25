namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Enums;
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

        public IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix)
        {
            LuceneSearch.Index = Index.BeachIndex;
            var results = LuceneSearch.SearchByPrefix(prefix, 10);

            return results;
        }

        public void AddBeachToIndex(Beach beach)
        {
            LuceneSearch.Index = Index.BeachIndex;

            LuceneSearch.AddUpdateIndexEntry(beach);
        }
    }
}