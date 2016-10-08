namespace BeachRankings.Data.UnitOfWork
{
    using BeachRankings.Data.Repositories;
    using BeachRankings.Models;

    public interface IBeachRankingsData
    {
        IRepository<User> Users { get; }

        IRepository<Beach> Beaches { get; }

        IRepository<BeachPhoto> BeachPhotos { get; }

        IRepository<Review> Reviews { get; }
    }
}