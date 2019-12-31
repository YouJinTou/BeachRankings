﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BR.Core.Abstractions;
using BR.Core.Extensions;
using BR.Core.Models;
using BR.Core.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Core.Cloud.Aws
{
    public class DynamoRepository<T> : INoSqlRepository<T> where T : IDbModel
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

            if (document == null)
            {
                throw new KeyNotFoundException(
                    $"Could not find {partitionKey}" +
                    $"{(string.IsNullOrWhiteSpace(sortKey) ? string.Empty : sortKey)}");
            }

            var item = document.ConvertTo<T>();

            return item;
        }

        public async Task<IEnumerable<T>> GetManyAsync(
            string partitionKeyName, 
            string partitionKeyValue, 
            string sortKeyName = null, 
            string sortKeyValue = null,
            NoSqlQueryOperator? op = null)
        {
            Validator.ThrowIfAnyNullOrWhiteSpace(partitionKeyName, partitionKeyValue);

            var allNull = Validator.AllNull(sortKeyName, sortKeyValue, op);

            if (!allNull)
            {
                Validator.ThrowIfAnyNullOrWhiteSpace(sortKeyName, sortKeyValue, op?.ToString());
            }

            var kce = $"{partitionKeyName} = :v_{partitionKeyName}";
            kce = allNull ? 
                kce : 
                $"{kce} and {sortKeyName} {op.Value.AsDynamoString()} :v_{sortKeyValue}";
            var request = new QueryRequest
            {
                TableName = this.tableName,
                KeyConditionExpression = kce,
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

        public async Task<IEnumerable<T>> GetManyByAttributeAsync(
           string partitionKey, string attributeName, string attributeValue)
        {
            Validator.ThrowIfAnyNullOrWhiteSpace(partitionKey, attributeName, attributeValue);

            var table = Table.LoadTable(this.client, this.tableName);
            var filter = new QueryFilter();

            filter.AddCondition(attributeName, QueryOperator.Equal, attributeValue);

            var searchQuery = table.Query(partitionKey, filter);
            var docs = await searchQuery.GetNextSetAsync();
            var items = docs.Select(d => this.ConvertTo(d));

            return items;
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
            var batchSize = 50;
            var count = 0;

            foreach (var itemBatch in items.ToBatches(batchSize))
            {
                await this.WriteBatchAsync(table, itemBatch);

                await Task.Delay(2000);

                count += batchSize;
            }
        }

        public async Task UpdateAsync(T item)
        {
            Validator.ThrowIfNull(item, "Cannot update item.");

            var table = Table.LoadTable(this.client, this.tableName);
            var document = item.ToDynamoDbDocument();

            await table.UpdateItemAsync(document);
        }

        protected virtual T ConvertTo(Document doc)
        {
            return doc.ConvertTo<T>();
        }

        protected virtual IEnumerable<T> ConvertItemsTo(
            IEnumerable<Dictionary<string, AttributeValue>> items)
        {
            foreach (var item in items)
            {
                yield return item.ConvertTo<T>();
            }
        }

        private async Task WriteBatchAsync(Table table, IEnumerable<T> itemBatch)
        {
            try
            {
                var batch = table.CreateBatchWrite();

                foreach (var item in itemBatch)
                {
                    batch.AddDocumentToPut(item.ToDynamoDbDocument());

                    System.Console.WriteLine(item.ToString());
                }

                await batch.ExecuteAsync();
            }
            catch (ProvisionedThroughputExceededException)
            {
                await Task.Delay(20000);

                await this.WriteBatchAsync(table, itemBatch);
            }
        }
    }
}
