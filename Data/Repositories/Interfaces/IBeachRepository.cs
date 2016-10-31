namespace BeachRankings.Data.Repositories.Interfaces
{
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;

    public interface IBeachRepository : IGenericRepository<Beach>, IPlaceRepository
    {
        void DeleteIndexEntry(ISearchable searchable);
    }
}