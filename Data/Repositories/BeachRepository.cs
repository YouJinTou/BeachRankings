namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Enums;
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

        public IEnumerable<ISearchable> GetBeachIdsByQuery(string query, string fieldName = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                return null;
            }

            var terms = query.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            query = string.Join(" ", terms);
            LuceneSearch.Index = Index.BeachIndex;

            return LuceneSearch.Search(query, fieldName);
        }

        public IEnumerable<ISearchable> GetTermsByKeystroke(string prefix)
        {
            LuceneSearch.Index = Index.BeachIndex;
            var results = LuceneSearch.SearchByPrefix(prefix, 10);

            return results;
        }

        public void AddBeachToIndex(Beach beach)
        {
            LuceneSearch.Index = Index.BeachIndex;

            LuceneSearch.AddUpdateIndexEntry(beach);
        }
    }
}