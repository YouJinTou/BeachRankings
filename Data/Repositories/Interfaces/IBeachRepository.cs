namespace BeachRankings.Data.Repositories.Interfaces
{
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using System.Linq;

    public interface IBeachRepository : IGenericRepository<Beach>, IPlaceRepository
    {
        IQueryable<Beach> OrderByCriterion(int criterion);

        //IOrderedQueryable<Beach> OrderByCriterion(int criterion);

        IQueryable<Beach> FilterByCountry(int id);

        IQueryable<Beach> FilterByWaterBody(int id);
        
        void DeleteIndexEntry(ISearchable searchable);
    }
}