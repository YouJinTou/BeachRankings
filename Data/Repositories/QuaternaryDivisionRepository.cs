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

    public class QuaternaryDivisionRepository : GenericRepository<QuaternaryDivision>, IQuaternaryDivisionRepository
    {
        private DbContext dbContext;
        private IDbSet<QuaternaryDivision> entitySet;

        public QuaternaryDivisionRepository(DbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = dbContext.Set<QuaternaryDivision>();
        }
        
        public IEnumerable<PlaceSearchResultModel> GetSearchResultsByKeyStroke(string prefix)
        {
            LuceneSearch.Index = Index.QuaternaryDivisionIndex;
            var searchables = LuceneSearch.SearchByPrefix(prefix, 10);
            var results = new List<PlaceSearchResultModel>();

            foreach (var searchble in searchables)
            {
                results.Add((PlaceSearchResultModel)searchble);
            }

            return results.Where(r => r.BeachCount > 0).OrderByDescending(r => r.BeachCount);
        }

        public void AddDivisionToIndex(ISearchable quaternaryDivision)
        {
            LuceneSearch.Index = Index.QuaternaryDivisionIndex;

            LuceneSearch.AddUpdateIndexEntry(quaternaryDivision);
        }
    }
}