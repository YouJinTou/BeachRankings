namespace BeachRankings.Data.Repositories.Interfaces
{
    using BeachRankings.Models;

    public interface ICountryRepository : IGenericRepository<Country>, IPlaceRepository
    {
    }
}