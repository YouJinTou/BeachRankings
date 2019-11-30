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
    public class DynamoRepository<T> : INoSqlRepository<T> where T : IDbModel
    {
        private readonly Table table;
        private readonly AmazonDynamoDBClient client;
        private readonly string tableName;

        public DynamoRepository(IAmazonDynamoDB dynamoDb, string table)
        {
            this.table = Table.LoadTable(dynamoDb, table);
            this.client = new AmazonDynamoDBClient();
            this.tableName = table;
        }

        public async Task<T> GetAsync(string partitionKey, string sortKey = null)
        {
            Validator.ThrowIfNullOrWhiteSpace(partitionKey);

            var document = string.IsNullOrWhiteSpace(sortKey) ?
                await this.table.GetItemAsync(new Primitive(partitionKey)) :
                await this.table.GetItemAsync(new Primitive(partitionKey), new Primitive(sortKey));
            var item = document.ConvertTo<T>();

            return item;
        }

        public async Task<IEnumerable<T>> GetManyAsync(string partitionKey)
        {
            Validator.ThrowIfNullOrWhiteSpace(partitionKey);

            var client = new AmazonDynamoDBClient();
            var request = new QueryRequest
            {
                TableName = this.tableName,
                KeyConditionExpression = "id = :v_id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {
                        ":v_id", new AttributeValue { S =  partitionKey }
                    }
                }
            };
            var response = await client.QueryAsync(request);
            var result = new List<T>();

            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
            }

            return result;
        }

        public async Task AddAsync(T item)
        {
            Validator.ThrowIfNull(item);

            var document = item.ToDynamoDbDocument();

            await this.table.PutItemAsync(document);
        }
    }
}
