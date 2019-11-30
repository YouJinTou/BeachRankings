using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface INoSqlRepository<T> where T : IDbModel
    {
        Task<T> GetAsync(string partitionKey, string sortKey = null);

        Task<IEnumerable<T>> GetManyAsync(string partitionKey);

        Task AddAsync(T item);
    }
}
