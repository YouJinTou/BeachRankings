namespace BeachRankings.Data.UnitOfWork
{
    using BeachRankings.Data.Repositories;
    using BeachRankings.Models;

    public interface IBeachRankingsData
    {
        IGenericRepository<User> Users { get; }

        IBeachRepository Beaches { get; }

        IGenericRepository<BeachPhoto> BeachPhotos { get; }

        IGenericRepository<Review> Reviews { get; }
    }
}