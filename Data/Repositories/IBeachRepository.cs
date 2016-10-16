namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using System.Collections.Generic;

    public interface IBeachRepository : IGenericRepository<Beach>
    {
        IEnumerable<int> GetBeachIdsByQuery(string query, string fieldName = null);

        IEnumerable<int> GetTermsByKeystroke(string prefix);

        void AddBeachToIndex(Beach beach);
    }
}