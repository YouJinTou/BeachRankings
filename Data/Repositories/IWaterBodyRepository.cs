namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;

    public interface IWaterBodyRepository : IGenericRepository<WaterBody>
    {
        void AddWaterBodyToIndex(WaterBody waterBody);
    }
}