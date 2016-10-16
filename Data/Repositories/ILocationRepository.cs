namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;

    public interface ILocationRepository : IGenericRepository<Location>
    {
        void AddLocationToIndex(Location location);
    }
}