using BeachRankings.Api.Abstractions;
using BeachRankings.Core.Abstractions;
using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeachRankings.Api.Services
{
    internal class SearchService : ISearchService
    {
        private readonly IBeachesRepository beachesRepository;

        public SearchService(IBeachesRepository beachesRepository)
        {
            this.beachesRepository = beachesRepository;
        }

        public async Task<IEnumerable<BeachDbResultModel>> FindBeachesAsync(BeachQueryModel model)
        {
            var continents = await this.beachesRepository.GetManyAsync(model);
            var countries = await this.beachesRepository.GetManyAsync(model);
            var l1s = await this.beachesRepository.GetManyAsync(model);
            var l2s = await this.beachesRepository.GetManyAsync(model);
            var l3s = await this.beachesRepository.GetManyAsync(model);
            var l4s = await this.beachesRepository.GetManyAsync(model);
            var waterBodies = await this.beachesRepository.GetManyAsync(model);
            var beaches = await this.beachesRepository.GetManyAsync(model);
            var models = new List<BeachDbResultModel>();

            models.AddRange(continents);
            models.AddRange(countries);
            models.AddRange(l1s);
            models.AddRange(l2s);
            models.AddRange(l3s);
            models.AddRange(l4s);
            models.AddRange(waterBodies);
            models.AddRange(beaches);

            return models;
        }
    }
}
