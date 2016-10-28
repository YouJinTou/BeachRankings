namespace BeachRankings.Data.Repositories.Interfaces
{
    using BeachRankings.Models;
    using BeachRankings.Services.Search.Models;
    using System.Collections.Generic;

    public interface IWaterBodyRepository : IGenericRepository<WaterBody>
    {
        IEnumerable<PlaceSearchResultModel> GetSearchResultsByKeyStroke(string prefix);

        void AddWaterBodyToIndex(WaterBody waterBody);
    }
}