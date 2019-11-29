using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    internal interface INoSqlRepository<T>
    {
        Task<IEnumerable<T>> GetManyAsync(string partitionKey);
    }
}
