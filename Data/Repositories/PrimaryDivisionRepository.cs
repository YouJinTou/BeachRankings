﻿namespace BeachRankings.Data.Repositories
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

    public class PrimaryDivisionRepository : GenericRepository<PrimaryDivision>, IPrimaryDivisionRepository
    {
        private DbContext dbContext;
        private IDbSet<PrimaryDivision> entitySet;

        public PrimaryDivisionRepository(DbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = dbContext.Set<PrimaryDivision>();
        }
        
        public IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix)
        {
            var searchService = new LuceneSearch(Index.PrimaryDivisionIndex);
            var searchables = searchService.SearchByPrefix(prefix, 10);
            var results = new List<PrimaryDivisionSearchResultModel>();

            foreach (var searchable in searchables)
            {
                results.Add((PrimaryDivisionSearchResultModel)searchable);
            }

            return results.Where(pd => pd.BeachCount > 0).OrderByDescending(r => r.BeachCount);
        }

        public void AddUpdateIndexEntry(ISearchable primaryDivision)
        {
            var searchService = new LuceneSearch(Index.PrimaryDivisionIndex);

            searchService.AddUpdateIndexEntry(primaryDivision);
        }

        public void DeleteIndexEntry(ISearchable primaryDivision)
        {
            var searchService = new LuceneSearch(Index.PrimaryDivisionIndex);

            searchService.DeleteIndexEntry(primaryDivision);
        }
    }
}