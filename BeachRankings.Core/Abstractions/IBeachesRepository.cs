using BeachRankings.Core.DAL;
using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeachRankings.Core.Abstractions
{
    public interface IBeachesRepository : IRepository<Beach>
    {
        Task<IEnumerable<BeachDbResultModel>> GetManyAsync(
            BeachPartitionKey key,
            string pf = null,
            string ct = null,
            string cy = null,
            string l1 = null,
            string l2 = null,
            string l3 = null,
            string l4 = null,
            string wb = null,
            string orderBy = null,
            string orderDirection = null);
    }
}
