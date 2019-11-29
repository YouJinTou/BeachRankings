using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BR.Core.Abstractions;
using BR.Core.Tools;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Cloud.Aws
{
    internal class DynamoRepository<T> : INoSqlRepository<T>
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
    }
}
