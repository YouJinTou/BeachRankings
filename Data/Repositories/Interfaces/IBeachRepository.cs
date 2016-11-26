namespace BeachRankings.Data.Repositories.Interfaces
{
    using BeachRankings.Models;
    using System.Linq;

    public interface IBeachRepository : IGenericRepository<Beach>, IPlaceRepository
    {
        IQueryable<Beach> OrderByCriterion(int criterion);

        IQueryable<Beach> FilterByCountry(int id);

        IQueryable<Beach> FilterByWaterBody(int id);
    }
}