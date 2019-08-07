﻿using Amazon.DynamoDBv2;
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

        public async Task<IEnumerable<Beach>> GetManyAsync(BeachQueryModel model)
        {
            InputValidator.ThrowIfNull(model);

            var partitionKey = new Primitive(Beach.PrimaryPartitionKeyType.ToString());
            var prefix = new QueryPrefix(model.PF);
            var filter = new QueryFilter()
                .TryAddCondition(nameof(Beach.Location), QueryOperator.BeginsWith, prefix)
                .TryAddCondition(nameof(Beach.Continent), QueryOperator.Equal, model.CT)
                .TryAddCondition(nameof(Beach.Country), QueryOperator.Equal, model.CY)
                .TryAddCondition(nameof(Beach.L1), QueryOperator.Equal, model.L1)
                .TryAddCondition(nameof(Beach.L2), QueryOperator.Equal, model.L2)
                .TryAddCondition(nameof(Beach.L3), QueryOperator.Equal, model.L3)
                .TryAddCondition(nameof(Beach.L4), QueryOperator.Equal, model.L4)
                .TryAddCondition(nameof(Beach.WaterBody), QueryOperator.Equal, model.WB);
            var searchQuery = this.table.Query(partitionKey, filter);
            var docs = await searchQuery.GetNextSetAsync();
            var beaches = this.mapper.Map<IEnumerable<Beach>>(docs);
            var prop = typeof(Beach).TryGetProperty(model.OB, nameof(Beach.Score));
            var orderedBeaches = (model.OD.ToSortDirection() == Constants.View.Descending) ? 
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
            var beaches = this.mapper.Map<IEnumerable<Beach>>(docs);

            return beaches;
        }
    }
}
