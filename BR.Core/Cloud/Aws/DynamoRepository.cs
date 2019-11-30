using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BR.Core.Abstractions;
using BR.Core.Extensions;
using BR.Core.Tools;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Cloud.Aws
{
    internal class DynamoRepository<T> : INoSqlRepository<T> where T : IDbModel
    {
        private readonly AmazonDynamoDBClient client;
        private readonly string tableName;

        public DynamoRepository(string table)
        {
            this.client = new AmazonDynamoDBClient();
            this.tableName = table;
        }

        public async Task<T> GetAsync(string partitionKey, string sortKey = null)
        {
            Validator.ThrowIfNullOrWhiteSpace(partitionKey);

            var table = Table.LoadTable(this.client, this.tableName);
            var document = string.IsNullOrWhiteSpace(sortKey) ?
                await table.GetItemAsync(new Primitive(partitionKey)) :
                await table.GetItemAsync(new Primitive(partitionKey), new Primitive(sortKey));
            var item = document.ConvertTo<T>();

            return item;
        }

        public async Task<IEnumerable<T>> GetManyAsync(
            string partitionKeyValue, string partitionKeyName = Constants.StreamId)
        {
            Validator.ThrowIfAnyNullOrWhiteSpace(partitionKeyName, partitionKeyValue);

            var request = new QueryRequest
            {
                TableName = this.tableName,
                KeyConditionExpression = $"{partitionKeyName} = :v_{partitionKeyName}",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {
                    $":v_{partitionKeyName}", new AttributeValue { S = partitionKeyValue }
                }
            }
            };
            var response = await this.client.QueryAsync(request);
            var result = this.ConvertItemsTo(response.Items);

            return result;
        }

        public async Task AddAsync(T item)
        {
            Validator.ThrowIfNull(item);

            var table = Table.LoadTable(this.client, this.tableName);
            var document = item.ToDynamoDbDocument();

            await table.PutItemAsync(document);
        }

        public async Task AddManyAsync(IEnumerable<T> items)
        {
            Validator.ThrowIfNull(items);

            var table = Table.LoadTable(this.client, this.tableName);
            var batch = table.CreateBatchWrite();

            foreach (var item in items)
            {
                batch.AddDocumentToPut(item.ToDynamoDbDocument());
            }

            await batch.ExecuteAsync();
        }

        protected virtual IEnumerable<T> ConvertItemsTo(
            IEnumerable<Dictionary<string, AttributeValue>> items)
        {
            foreach (var item in items)
            {
                yield return item.ConvertTo<T>();
            }
        }
    }
}
