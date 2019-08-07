using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
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
        private readonly IMapper mapper;

        public BeachesRepository(IAmazonDynamoDB dynamoDb, string table, IMapper mapper)
            : base(dynamoDb, table)
        {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BeachDbResultModel>> GetManyAsync(BeachQueryModel model)
        {
            InputValidator.ThrowIfNull(model);

            var partitionKey = new Primitive(Beach.PrimaryPartitionKeyType.ToString());
            var filter = new QueryFilter()
                .TryAddCondition(nameof(Beach.Location), QueryOperator.BeginsWith, model.PF)
                .TryAddCondition(nameof(Beach.Continent), QueryOperator.Equal, model.CT)
                .TryAddCondition(nameof(Beach.Country), QueryOperator.Equal, model.CY)
                .TryAddCondition(nameof(Beach.L1), QueryOperator.Equal, model.L1)
                .TryAddCondition(nameof(Beach.L2), QueryOperator.Equal, model.L2)
                .TryAddCondition(nameof(Beach.L3), QueryOperator.Equal, model.L3)
                .TryAddCondition(nameof(Beach.L4), QueryOperator.Equal, model.L4)
                .TryAddCondition(nameof(Beach.WaterBody), QueryOperator.Equal, model.WB);
            var searchQuery = this.table.Query(partitionKey, filter);
            var docs = await searchQuery.GetNextSetAsync();
            var models = this.mapper.Map<IEnumerable<BeachDbResultModel>>(docs);
            var orderDirection = model.OD.ToSortDirection();
            var prop = typeof(BeachDbResultModel).TryGetProperty(model.OB, nameof(Beach.Score));
            models = model.OD.ToSortDirection() == Constants.View.Descending ? 
                models.OrderByDescending(m => prop.GetValue(m, null)) : 
                models.OrderBy(m => prop.GetValue(m, null));

            return models;
        }
    }
}
