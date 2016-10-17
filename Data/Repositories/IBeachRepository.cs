namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;

    public interface IBeachRepository : IGenericRepository<Beach>
    {
        IEnumerable<ISearchable> GetSearchResults(string query, string fieldName = null);

        IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix);

        void AddBeachToIndex(Beach beach);
    }
}