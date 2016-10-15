namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.Data.Entity;

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

        public IList<string> Search(string query)
        {
            return new List<string>() { "flower" };
        }
    }
}