using AutoMapper;
using BeachRankings.Api.Abstractions;
using BeachRankings.Api.Models.Beaches;
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

        public async Task<SearchViewModel> SearchPlacesAsync(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                return new SearchViewModel();
            }

            var tasks = new List<Task<IEnumerable<Beach>>>
            {
                this.beachesRepository.GetManyAsync(BeachPartitionKey.L4, prefix),
                this.beachesRepository.GetManyAsync(BeachPartitionKey.L3, prefix),
                this.beachesRepository.GetManyAsync(BeachPartitionKey.L2, prefix),
                this.beachesRepository.GetManyAsync(BeachPartitionKey.L1, prefix),
                this.beachesRepository.GetManyAsync(BeachPartitionKey.Country, prefix),
                this.beachesRepository.GetManyAsync(BeachPartitionKey.WaterBody, prefix),
                this.beachesRepository.GetManyAsync(BeachPartitionKey.Continent, prefix),
                this.beachesRepository.GetManyAsync(BeachPartitionKey.Beach, prefix)
            };
            var beaches = (await Task.WhenAll(tasks)).SelectMany(c => c.Select(b => b));

            return new SearchViewModel(beaches, prefix, this.mapper);
        }

        public async Task<IEnumerable<BeachDbResultModel>> SearchBeachesAsync(BeachQueryModel model)
        {
            InputValidator.ThrowIfNull(model);

            if (!model.IsValid())
            {
                return new List<BeachDbResultModel>();
            }

            var beaches = await this.beachesRepository.GetManyAsync(model);
            var models = this.mapper.Map<IEnumerable<BeachDbResultModel>>(beaches);

            return models;
        }
    }
}
