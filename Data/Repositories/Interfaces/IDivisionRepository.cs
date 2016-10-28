namespace BeachRankings.Data.Repositories.Interfaces
{
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Search.Models;
    using System.Collections.Generic;

    public interface IDivisionRepository
    {
        IEnumerable<PlaceSearchResultModel> GetSearchResultsByKeyStroke(string prefix);

        void AddDivisionToIndex(ISearchable searchable);
    }
}