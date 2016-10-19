namespace BeachRankings.Data.UnitOfWork
{
    using BeachRankings.Data.Repositories;
    using BeachRankings.Models;

    public interface IBeachRankingsData
    {
        IGenericRepository<User> Users { get; }

        IWaterBodyRepository WaterBodies { get; }

        ILocationRepository Locations { get; }

        IBeachRepository Beaches { get; }

        IGenericRepository<BeachPhoto> BeachPhotos { get; }

        IGenericRepository<Review> Reviews { get; }
    }
}