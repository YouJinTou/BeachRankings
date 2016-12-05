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
            LuceneSearch.Index = Index.ContinentIndex;
            var searchables = LuceneSearch.SearchByPrefix(prefix, 10);
            var results = new List<PlaceSearchResultModel>();

            foreach (var searchable in searchables)
            {
                results.Add((PlaceSearchResultModel)searchable);
            }

            return results.Where(r => r.BeachCount > 0).OrderByDescending(r => r.BeachCount);
        }

        public void AddUpdateIndexEntry(ISearchable continent)
        {
            LuceneSearch.Index = Index.ContinentIndex;

            LuceneSearch.AddUpdateIndexEntry(continent);
        }

        public void DeleteIndexEntry(ISearchable continent)
        {
            LuceneSearch.Index = Index.ContinentIndex;

            LuceneSearch.DeleteIndexEntry(continent);
        }
    }
}