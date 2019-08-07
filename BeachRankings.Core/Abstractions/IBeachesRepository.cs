using BeachRankings.Core.DAL;
using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeachRankings.Core.Abstractions
{
    public interface IBeachesRepository : IRepository<Beach>
    {
        Task<IEnumerable<Beach>> GetManyAsync(BeachPartitionKey key, string prefix);

        Task<IEnumerable<Beach>> GetManyAsync(BeachQueryModel model);
    }
}
