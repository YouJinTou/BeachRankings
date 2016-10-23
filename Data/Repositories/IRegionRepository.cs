namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;

    public interface IRegionRepository : IGenericRepository<Region>
    {
        IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix);
       
        void AddRegionToIndex(Region region);
    }
}