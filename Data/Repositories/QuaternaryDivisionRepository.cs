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
        
        public IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix)
        {
            var searchService = new LuceneSearch(Index.QuaternaryDivisionIndex);
            var searchables = searchService.SearchByPrefix(prefix, 10);
            var results = new List<QuaternaryDivisionSearchResultModel>();

            foreach (var searchable in searchables)
            {
                results.Add((QuaternaryDivisionSearchResultModel)searchable);
            }

            return results.Where(qd => qd.BeachCount > 0).OrderByDescending(r => r.BeachCount);
        }

        public void AddUpdateIndexEntry(ISearchable quaternaryDivision)
        {
            var searchService = new LuceneSearch(Index.QuaternaryDivisionIndex);

            searchService.AddUpdateIndexEntry(quaternaryDivision);
        }

        public void DeleteIndexEntry(ISearchable quaternaryDivision)
        {
            var searchService = new LuceneSearch(Index.QuaternaryDivisionIndex);

            searchService.DeleteIndexEntry(quaternaryDivision);
        }
    }
}