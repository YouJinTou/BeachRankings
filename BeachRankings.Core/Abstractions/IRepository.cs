using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeachRankings.Core.Abstractions
{
    public interface IRepository<T> where T : IDbModel
    {
        Task<T> GetAsync(object id);

        Task AddAsync(T item);

        Task AddManyAsync(IEnumerable<T> items, int batchSize = 100, int coolDown = 1000);
    }
}
