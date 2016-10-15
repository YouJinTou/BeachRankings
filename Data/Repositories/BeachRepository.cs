namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using BeachRankings.Data.Services.Search;
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

        public IEnumerable<int> GetBeachIdsByQuery(string query, string fieldName = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                return null;
            }

            var terms = query.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            query = string.Join(" ", terms);
          
            return LuceneSearch.Search(query, fieldName);
        }
    }
}