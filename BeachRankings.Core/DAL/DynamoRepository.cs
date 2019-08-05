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
        private readonly IAmazonDynamoDB dynamoDb;
        private readonly string table;

        public DynamoRepository(IAmazonDynamoDB dynamoDb, string table)
        {
            this.dynamoDb = dynamoDb;
            this.table = table;
        }

        public async Task<T> GetAsync(object id)
        {
            InputValidator.ThrowIfNull(id);

            var table = Table.LoadTable(this.dynamoDb, this.table);
            var partitionKey = id.ToString();
            var sortKey = partitionKey.Replace(Constants.DynamoPartitionKeyPrefix, string.Empty);
            var document = 
                await table.GetItemAsync(new Primitive(partitionKey), new Primitive(sortKey));

            return document.ConvertTo<T>();
        }

        public async Task AddAsync(T item)
        {
            var table = Table.LoadTable(this.dynamoDb, this.table);

            await table.PutItemAsync(item.ToDynamoDbDocument());
        }

        public async Task AddManyAsync(
            IEnumerable<T> items, int batchSize = 100, int coolDown = 1000)
        {
            var table = Table.LoadTable(this.dynamoDb, this.table);
            var page = 0;

            while (true)
            {
                var itemsBatch = items.Skip(page * batchSize).Take(batchSize);

                if (itemsBatch.IsEmpty())
                {
                    return;
                }

                var batch = table.CreateBatchWrite();

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
