namespace BeachRankings.Data.Repositories.Interfaces
{
    using BeachRankings.Models;

    public interface IBeachRepository : IGenericRepository<Beach>, IPlaceRepository
    {
    }
}