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

    public class SecondaryDivisionRepository : GenericRepository<SecondaryDivision>, ISecondaryDivisionRepository
    {
        private DbContext dbContext;
        private IDbSet<SecondaryDivision> entitySet;

        public SecondaryDivisionRepository(DbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = dbContext.Set<SecondaryDivision>();
        }
        
        public IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix)
        {
            var searchService = new LuceneSearch(Index.SecondaryDivisionIndex);
            var searchables = searchService.SearchByPrefix(prefix, 10);
            var results = new List<SecondaryDivisionSearchResultModel>();

            foreach (var searchable in searchables)
            {
                results.Add((SecondaryDivisionSearchResultModel)searchable);
            }

            return results.Where(sd => sd.BeachCount > 0).OrderByDescending(r => r.BeachCount);
        }

        public void AddUpdateIndexEntry(ISearchable secondaryDivision)
        {
            var searchService = new LuceneSearch(Index.SecondaryDivisionIndex);

            searchService.AddUpdateIndexEntry(secondaryDivision);
        }

        public void DeleteIndexEntry(ISearchable secondaryDivision)
        {
            var searchService = new LuceneSearch(Index.SecondaryDivisionIndex);

            searchService.DeleteIndexEntry(secondaryDivision);
        }
    }
}