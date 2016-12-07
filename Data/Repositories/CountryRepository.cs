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

    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private DbContext dbContext;
        private IDbSet<Country> entitySet;

        public CountryRepository(DbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = dbContext.Set<Country>();
        }
        
        public IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix)
        {
            var searchService = new LuceneSearch(Index.CountryIndex);
            var searchables = searchService.SearchByPrefix(prefix, 10);
            var results = new List<CountrySearchResultModel>();

            foreach (var searchable in searchables)
            {
                results.Add((CountrySearchResultModel)searchable);
            }

            return results.Where(c => c.BeachCount > 0).OrderByDescending(r => r.BeachCount);
        }

        public void AddUpdateIndexEntry(ISearchable country)
        {
            var searchService = new LuceneSearch(Index.CountryIndex);

            searchService.AddUpdateIndexEntry(country);
        }

        public void DeleteIndexEntry(ISearchable country)
        {
            var searchService = new LuceneSearch(Index.CountryIndex);

            searchService.DeleteIndexEntry(country);
        }
    }
}