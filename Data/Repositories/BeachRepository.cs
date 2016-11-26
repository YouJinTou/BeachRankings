namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Data.Extensions;
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Enums;
    using BeachRankings.Services.Search.Models;
    using BeachRankings.Data.Repositories.Interfaces;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

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
            var searchables = LuceneSearch.SearchByPrefix(prefix, 10);
            var results = new List<BeachSearchResultModel>();

            foreach (var searchable in searchables)
            {
                results.Add((BeachSearchResultModel)searchable);
            }

            return results;
        }

        public IQueryable<Beach> OrderByCriterion(int criterion)
        {
            return this.entitySet.OrderByCriterion(criterion);
        }

        public IQueryable<Beach> FilterByCountry(int id)
        {
            return this.entitySet.FilterByCountry(id);
        }

        public IQueryable<Beach> FilterByWaterBody(int id)
        {
            return this.entitySet.FilterByWaterBody(id);
        }

        public void AddUpdateIndexEntry(ISearchable beach)
        {
            LuceneSearch.Index = Index.BeachIndex;

            LuceneSearch.AddUpdateIndexEntry(beach);
        }

        public void DeleteIndexEntry(ISearchable beach)
        {
            LuceneSearch.Index = Index.BeachIndex;

            LuceneSearch.DeleteIndexEntry(beach);
        }
    }
}