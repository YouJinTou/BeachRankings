using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using BeachRankings.Core.Abstractions;
using BeachRankings.Core.Extensions;
using BeachRankings.Core.Models;
using BeachRankings.Core.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachRankings.Core.DAL
{
    internal class BeachesRepository : DynamoRepository<Beach>, IBeachesRepository
    {
        public BeachesRepository(IAmazonDynamoDB dynamoDb, string table)
            : base(dynamoDb, table)
        {
        }

        public async Task<IEnumerable<Beach>> GetManyAsync(BeachQueryModel model)
        {
            InputValidator.ThrowIfNull(model);

            var partitionKey = new Primitive(Beach.PrimaryPartitionKeyType.ToString());
            var prefix = new QueryPrefix(model.Prefix);
            var filter = new QueryFilter()
                .TryAddCondition(nameof(Beach.Location), QueryOperator.BeginsWith, prefix)
                .TryAddCondition(nameof(Beach.Continent), QueryOperator.Equal, model.Continent)
                .TryAddCondition(nameof(Beach.Country), QueryOperator.Equal, model.Country)
                .TryAddCondition(nameof(Beach.L1), QueryOperator.Equal, model.L1)
                .TryAddCondition(nameof(Beach.L2), QueryOperator.Equal, model.L2)
                .TryAddCondition(nameof(Beach.L3), QueryOperator.Equal, model.L3)
                .TryAddCondition(nameof(Beach.L4), QueryOperator.Equal, model.L4)
                .TryAddCondition(nameof(Beach.WaterBody), QueryOperator.Equal, model.WaterBody);
            var searchQuery = this.table.Query(partitionKey, filter);
            var docs = await searchQuery.GetNextSetAsync();
            var beaches = docs.Select(d => d.ConvertTo<Beach>());
            var prop = typeof(Beach).TryGetProperty(model.OrderBy, nameof(Beach.Score));
            var orderedBeaches = (model.SortDirection.ToSortDirection() == Constants.View.Descending) ? 
                beaches.OrderByDescending(m => prop.GetValue(m, null)) : 
                beaches.OrderBy(m => prop.GetValue(m, null));

            return orderedBeaches;
        }

        public async Task<IEnumerable<Beach>> GetManyAsync(
            BeachPartitionKey key, string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                return new List<Beach>();
            }

            prefix = new QueryPrefix(prefix);
            var partitionKey = new Primitive(key.ToString());
            var filter = new QueryFilter(nameof(Beach.Location), QueryOperator.BeginsWith, prefix);
            var searchQuery = this.table.Query(partitionKey, filter);
            var docs = await searchQuery.GetNextSetAsync();
            var beaches = docs.Select(d => d.ConvertTo<Beach>());

            return beaches;
        }
    }
}
