namespace BeachRankings.Data.Repositories.Interfaces
{
    using BeachRankings.Models;

    public interface IContinentRepository : IGenericRepository<Continent>, IPlaceRepository
    {
    }
}