﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface INoSqlRepository<T> where T : IDbModel
    {
        Task<T> GetAsync(string partitionKey, string sortKey = null);

        Task<IEnumerable<T>> GetManyAsync(string partitionKeyName, string partitionKeyValue);

        Task<IEnumerable<T>> GetManyAsync(
            string partitionKeyName, IEnumerable<string> partitionKeyValues);

        Task<IEnumerable<T>> GetManyBeginsWithAsync(
            string partitionKeyName,
            string partitionKeyValue,
            string sortKeyName,
            string sortKeyValue);

        Task<IEnumerable<T>> GetManyByAttributeAsync(
           string partitionKey, string attributeName, string attributeValue);

        Task AddAsync(T item);

        Task AddManyAsync(IEnumerable<T> items);

        Task UpdateAsync(T item);
    }
}
