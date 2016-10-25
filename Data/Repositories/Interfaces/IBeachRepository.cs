namespace BeachRankings.Data.Repositories.Interfaces
{
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;

    public interface IBeachRepository : IGenericRepository<Beach>
    {
        IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix);

        void AddBeachToIndex(Beach beach);
    }
}