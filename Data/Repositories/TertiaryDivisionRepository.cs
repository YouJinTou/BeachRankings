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

    public class TertiaryDivisionRepository : GenericRepository<TertiaryDivision>, ITertiaryDivisionRepository
    {
        private DbContext dbContext;
        private IDbSet<TertiaryDivision> entitySet;

        public TertiaryDivisionRepository(DbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = dbContext.Set<TertiaryDivision>();
        }
        
        public IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix)
        {
            var searchService = new LuceneSearch(Index.TertiaryDivisionIndex);
            var searchables = searchService.SearchByPrefix(prefix, 10);
            var results = new List<TertiaryDivisionSearchResultModel>();

            foreach (var searchable in searchables)
            {
                results.Add((TertiaryDivisionSearchResultModel)searchable);
            }

            return results.Where(td => td.BeachCount > 0).OrderByDescending(r => r.BeachCount);
        }

        public void AddUpdateIndexEntry(ISearchable tertiaryDivision)
        {
            var searchService = new LuceneSearch(Index.TertiaryDivisionIndex);

            searchService.AddUpdateIndexEntry(tertiaryDivision);
        }

        public void DeleteIndexEntry(ISearchable tertiaryDivision)
        {
            var searchService = new LuceneSearch(Index.TertiaryDivisionIndex);

            searchService.DeleteIndexEntry(tertiaryDivision);
        }
    }
}