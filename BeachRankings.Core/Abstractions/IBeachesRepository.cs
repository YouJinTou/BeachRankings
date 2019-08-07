using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeachRankings.Core.Abstractions
{
    public interface IBeachesRepository : IRepository<Beach>
    {
        Task<IEnumerable<BeachDbResultModel>> GetManyAsync(BeachQueryModel model);
    }
}
