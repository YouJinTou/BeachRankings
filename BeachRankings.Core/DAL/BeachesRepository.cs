using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using BeachRankings.Core.Abstractions;
using BeachRankings.Core.Extensions;
using BeachRankings.Core.Models;
using System.Collections.Generic;
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

        public async Task<IEnumerable<BeachDbResultModel>> GetManyAsync(
            BeachPartitionKey key, 
            string prefix = null,
            string continent = null,
            string country = null,
            string l1 = null,
            string l2 = null,
            string l3 = null,
            string l4 = null,
            string waterBody = null,
            string orderBy = null,
            string orderDirection = null)
        {
            var partitionKey = new Primitive(key.ToString());
            var filter = new QueryFilter()
                .TryAddCondition(nameof(Beach.Location), QueryOperator.BeginsWith, prefix)
                .TryAddCondition(nameof(Beach.Continent), QueryOperator.Equal, continent)
                .TryAddCondition(nameof(Beach.Country), QueryOperator.Equal, country)
                .TryAddCondition(nameof(Beach.L1), QueryOperator.Equal, l1)
                .TryAddCondition(nameof(Beach.L2), QueryOperator.Equal, l2)
                .TryAddCondition(nameof(Beach.L3), QueryOperator.Equal, l3)
                .TryAddCondition(nameof(Beach.L4), QueryOperator.Equal, l4)
                .TryAddCondition(nameof(Beach.WaterBody), QueryOperator.Equal, waterBody);
            var searchQuery = this.table.Query(partitionKey, filter);
            var docs = await searchQuery.GetNextSetAsync();
            var models = this.mapper.Map<IEnumerable<BeachDbResultModel>>(docs);

            return models;
        }
    }
}
