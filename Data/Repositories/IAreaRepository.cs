namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;

    public interface IAreaRepository : IGenericRepository<Area>
    {
        IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix);

        void AddAreaToIndex(Area area);
    }
}