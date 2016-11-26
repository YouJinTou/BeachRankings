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
            LuceneSearch.Index = Index.CountryIndex;
            var searchables = LuceneSearch.SearchByPrefix(prefix, 10);
            var results = new List<PlaceSearchResultModel>();

            foreach (var searchable in searchables)
            {
                results.Add((PlaceSearchResultModel)searchable);
            }

            return results.Where(r => r.BeachCount > 0).OrderByDescending(r => r.BeachCount);
        }

        public void AddUpdateIndexEntry(ISearchable country)
        {
            LuceneSearch.Index = Index.CountryIndex;

            LuceneSearch.AddUpdateIndexEntry(country);
        }

        public void DeleteIndexEntry(ISearchable country)
        {
            LuceneSearch.Index = Index.CountryIndex;

            LuceneSearch.DeleteIndexEntry(country);
        }
    }
}