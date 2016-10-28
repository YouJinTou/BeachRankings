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
        
        public IEnumerable<PlaceSearchResultModel> GetSearchResultsByKeyStroke(string prefix)
        {
            LuceneSearch.Index = Index.TertiaryDivisionIndex;
            var searchables = LuceneSearch.SearchByPrefix(prefix, 10);
            var results = new List<PlaceSearchResultModel>();

            foreach (var searchble in searchables)
            {
                results.Add((PlaceSearchResultModel)searchble);
            }

            return results.Where(r => r.BeachCount > 0).OrderByDescending(r => r.BeachCount);
        }

        public void AddDivisionToIndex(ISearchable tertiaryDivision)
        {
            LuceneSearch.Index = Index.TertiaryDivisionIndex;

            LuceneSearch.AddUpdateIndexEntry(tertiaryDivision);
        }
    }
}