namespace BeachRankings.Data.Repositories.Interfaces
{
    using BeachRankings.Models;

    public interface ITertiaryDivisionRepository : IGenericRepository<PrimaryDivision>, IDivisionRepository
    {
    }
}