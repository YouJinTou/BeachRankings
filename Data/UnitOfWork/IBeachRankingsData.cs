namespace BeachRankings.Data.UnitOfWork
{
    using BeachRankings.Data.Repositories;
    using BeachRankings.Models;

    public interface IBeachRankingsData
    {
        IGenericRepository<User> Users { get; }

        IGenericRepository<Country> Countries { get; }

        IWaterBodyRepository WaterBodies { get; }

        ILocationRepository Locations { get; }

        IBeachRepository Beaches { get; }

        IGenericRepository<BeachImage> BeachImages { get; }

        IGenericRepository<Review> Reviews { get; }
    }
}