using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using BeachRankings.Core.Abstractions;
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

        public async Task<IEnumerable<BeachQueryModel>> GetManyAsync(
            BeachPartitionKey key, string prefix)
        {
            var partitionKey = new Primitive(key.ToString());
            var filter = new QueryFilter(nameof(Beach.Location), QueryOperator.BeginsWith, prefix);
            var searchQuery = this.table.Query(partitionKey, filter);
            var docs = await searchQuery.GetNextSetAsync();
            var models = this.mapper.Map<IEnumerable<BeachQueryModel>>(docs);

            return models;
        }
    }
}
