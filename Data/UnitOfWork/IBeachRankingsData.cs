namespace BeachRankings.Data.UnitOfWork
{
    using Data.Repositories;
    using Models;

    public interface IBeachRankingsData
    {
        IRepository<User> Users { get; }

        IRepository<Review> Reviews { get; }
    }
}