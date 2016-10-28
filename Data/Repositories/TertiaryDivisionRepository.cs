namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Data.Repositories.Interfaces;
    using BeachRankings.Models;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Enums;
    using BeachRankings.Models.Interfaces;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System;

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
            throw new NotImplementedException();
        }

        public void AddDivisionToIndex(ISearchable searchable)
        {
            throw new NotImplementedException();
        }
    }
}