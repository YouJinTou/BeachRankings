namespace BeachRankings.Data.Repositories.Interfaces
{
    using BeachRankings.Models;
    using BeachRankings.Services.Search.Models;
    using System.Collections.Generic;

    public interface IBeachRepository : IGenericRepository<Beach>
    {
        IEnumerable<BeachSearchResultModel> GetSearchResultsByKeyStroke(string prefix);

        void AddBeachToIndex(Beach beach);
    }
}