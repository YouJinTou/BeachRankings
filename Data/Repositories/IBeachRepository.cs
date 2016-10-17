namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;

    public interface IBeachRepository : IGenericRepository<Beach>
    {
        IEnumerable<ISearchable> GetBeachIdsByQuery(string query, string fieldName = null);

        IEnumerable<ISearchable> GetTermsByKeystroke(string prefix);

        void AddBeachToIndex(Beach beach);
    }
}