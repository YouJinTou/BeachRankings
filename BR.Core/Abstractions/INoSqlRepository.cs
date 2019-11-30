using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface INoSqlRepository<T> where T : IDbModel
    {
        Task<IEnumerable<T>> GetManyAsync(string partitionKey);

        Task AddAsync(T item);
    }
}
