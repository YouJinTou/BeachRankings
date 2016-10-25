namespace BeachRankings.Data.UnitOfWork
{
    using BeachRankings.Data.Repositories;
    using BeachRankings.Models;

    public interface IBeachRankingsData
    {
        IGenericRepository<User> Users { get; }

        IGenericRepository<Country> Countries { get; }

        IDivisionRepository Divisions { get; }

        IRegionRepository Regions { get; }

        IAreaRepository Areas { get; }

        IBeachRepository Beaches { get; }

        IWaterBodyRepository WaterBodies { get; }

        IGenericRepository<BeachImage> BeachImages { get; }

        IGenericRepository<Review> Reviews { get; }
    }
}