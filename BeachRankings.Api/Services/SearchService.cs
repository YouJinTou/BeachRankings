using AutoMapper;
using BeachRankings.Api.Abstractions;
using BeachRankings.Core.Abstractions;
using BeachRankings.Core.DAL;
using BeachRankings.Core.Models;
using BeachRankings.Core.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachRankings.Api.Services
{
    internal class SearchService : ISearchService
    {
        private readonly IBeachesRepository beachesRepository;
        private readonly IMapper mapper;

        public SearchService(IBeachesRepository beachesRepository, IMapper mapper)
        {
            this.beachesRepository = beachesRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BeachDbResultModel>> FindBeachesAsync(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                return new List<BeachDbResultModel>();
            }

            var l4BeachesTask = this.beachesRepository.GetManyAsync(
                BeachPartitionKey.L4, prefix);
            var l3BeachesTask = this.beachesRepository.GetManyAsync(
                BeachPartitionKey.L3, prefix);
            var l2BeachesTask = this.beachesRepository.GetManyAsync(
                BeachPartitionKey.L2, prefix);
            var l1BeachesTask = this.beachesRepository.GetManyAsync(
                BeachPartitionKey.L1, prefix);
            var countryBeachesTask = this.beachesRepository.GetManyAsync(
                BeachPartitionKey.Country, prefix);
            var waterBodyBeachesTask = this.beachesRepository.GetManyAsync(
                BeachPartitionKey.WaterBody, prefix);
            var continentBeachesTask = this.beachesRepository.GetManyAsync(
                BeachPartitionKey.Continent, prefix);
            var beachesTask = this.beachesRepository.GetManyAsync(
                BeachPartitionKey.Beach, prefix);
            var tasks = new List<Task<IEnumerable<Beach>>>
            {
                l4BeachesTask,
                l3BeachesTask,
                l2BeachesTask,
                l1BeachesTask,
                countryBeachesTask,
                waterBodyBeachesTask,
                continentBeachesTask,
                beachesTask
            };
            var beaches = await Task.WhenAll(tasks);
            var models = this.mapper.Map<IEnumerable<IEnumerable<BeachDbResultModel>>>(beaches)
                .SelectMany(c => c.Select(x => x))
                .ToList();

            return models;
        }

        public async Task<IEnumerable<BeachDbResultModel>> FindBeachesAsync(BeachQueryModel model)
        {
            InputValidator.ThrowIfNull(model);

            if (!model.IsValid())
            {
                return new List<BeachDbResultModel>();
            }

            if (model.IsQueryByPrefixOnly())
            {
                return await this.FindBeachesAsync(model.PF);
            }

            var beaches = await this.beachesRepository.GetManyAsync(model);
            var models = this.mapper.Map<IEnumerable<BeachDbResultModel>>(beaches);

            return models;
        }
    }
}
