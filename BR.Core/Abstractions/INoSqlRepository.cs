using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    internal interface INoSqlRepository<T> where T : IDbModel
    {
        Task<T> GetAsync(string partitionKey, string sortKey = null);

        Task<IEnumerable<T>> GetManyAsync(
            string partitionKeyValue, string partitionKeyName = Constants.StreamId);

        Task<IEnumerable<T>> GetManyByAttributeAsync(
           string partitionKey, string attributeName, string attributeValue);

        Task AddAsync(T item);

        Task AddManyAsync(IEnumerable<T> items);
    }
}
