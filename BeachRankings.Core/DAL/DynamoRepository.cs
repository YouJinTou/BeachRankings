using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using BeachRankings.Core.Abstractions;
using BeachRankings.Core.Extensions;
using BeachRankings.Core.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachRankings.Core.DAL
{
    internal class DynamoRepository<T> : IRepository<T> where T : IDbModel
    {
        protected readonly Table table;

        public DynamoRepository(IAmazonDynamoDB dynamoDb, string table)
        {
            this.table = Table.LoadTable(dynamoDb, table);
        }

        public async Task<T> GetAsync(object id)
        {
            var primaryKey = id as PrimaryKey;

            InputValidator.ThrowIfNull(primaryKey);

            var partitionKey = new Primitive(primaryKey.PartitionKey);
            var sortKey = new Primitive(primaryKey.SortKey);
            var document = await this.table.GetItemAsync(partitionKey, sortKey);

            return document.ConvertTo<T>();
        }

        public async Task AddAsync(T item)
        {
            InputValidator.ThrowIfNull(item);

            await this.table.PutItemAsync(item.ToDynamoDbDocument());
        }

        public async Task AddManyAsync(
            IEnumerable<T> items, int batchSize = 100, int coolDown = 1000)
        {
            InputValidator.ThrowIfNull(items);

            InputValidator.ThrowIfAnyNotPositive(batchSize, coolDown);

            var page = 0;

            while (true)
            {
                var itemsBatch = items.Skip(page * batchSize).Take(batchSize);

                if (itemsBatch.IsEmpty())
                {
                    return;
                }

                var batch = this.table.CreateBatchWrite();

                foreach (var item in itemsBatch)
                {
                    batch.AddDocumentToPut(item.ToDynamoDbDocument());
                }

                await batch.ExecuteAsync();

                page++;

                await Task.Delay(coolDown);
            }
        }
    }
}
