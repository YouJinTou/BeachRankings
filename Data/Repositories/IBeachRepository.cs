namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using System.Collections.Generic;

    public interface IBeachRepository : IGenericRepository<Beach>
    {
        IList<string> Search(string query);
    }
}