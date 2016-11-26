namespace BeachRankings.Data.Repositories.Interfaces
{
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;

    public interface IPlaceRepository
    {
        IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix);

        void AddUpdateIndexEntry(ISearchable searchable);

        void DeleteIndexEntry(ISearchable searchable);
    }
}