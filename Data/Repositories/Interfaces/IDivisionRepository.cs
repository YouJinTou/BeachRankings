namespace BeachRankings.Data.Repositories.Interfaces
{
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;

    public interface IDivisionRepository
    {
        IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix);

        void AddDivisionToIndex(ISearchable searchable);
    }
}