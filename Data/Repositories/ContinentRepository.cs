namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Data.Repositories.Interfaces;
    using BeachRankings.Models;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Enums;
    using BeachRankings.Services.Search.Models;
    using BeachRankings.Models.Interfaces;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;

    public class ContinentRepository : GenericRepository<Continent>, IContinentRepository
    {
        private DbContext dbContext;
        private IDbSet<Continent> entitySet;

        public ContinentRepository(DbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = dbContext.Set<Continent>();
        }
        
        public IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix)
        {
            var searchService = new LuceneSearch(Index.ContinentIndex);
            var searchables = searchService.SearchByPrefix(prefix, 10);
            var results = new List<ContinentSearchResultModel>();

            foreach (var searchable in searchables)
            {
                results.Add((ContinentSearchResultModel)searchable);
            }

            return results.Where(c => c.BeachCount > 0).OrderByDescending(r => r.BeachCount);
        }

        public void AddUpdateIndexEntry(ISearchable continent)
        {
            var searchService = new LuceneSearch(Index.ContinentIndex);

            searchService.AddUpdateIndexEntry(continent);
        }

        public void DeleteIndexEntry(ISearchable continent)
        {
            var searchService = new LuceneSearch(Index.ContinentIndex);

            searchService.DeleteIndexEntry(continent);
        }
    }
}