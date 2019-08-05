using BeachRankings.Api.Abstractions;
using BeachRankings.Core.Abstractions;
using BeachRankings.Core.DAL;
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

        public async Task<IEnumerable<BeachQueryModel>> FindBeachesAsync(string prefix)
        {
            var continents = 
                await this.beachesRepository.GetManyAsync(BeachPartitionKey.Continent, prefix);
            var countries =
                await this.beachesRepository.GetManyAsync(BeachPartitionKey.Country, prefix);
            var l1s = 
                await this.beachesRepository.GetManyAsync(BeachPartitionKey.L1, prefix);
            var l2s = 
                await this.beachesRepository.GetManyAsync(BeachPartitionKey.L2, prefix);
            var l3s = 
                await this.beachesRepository.GetManyAsync(BeachPartitionKey.L3, prefix);
            var l4s = 
                await this.beachesRepository.GetManyAsync(BeachPartitionKey.L4, prefix);
            var waterBodies =
                await this.beachesRepository.GetManyAsync(BeachPartitionKey.WaterBody, prefix);
            var models = new List<BeachQueryModel>();

            models.AddRange(continents);
            models.AddRange(countries);
            models.AddRange(l1s);
            models.AddRange(l2s);
            models.AddRange(l3s);
            models.AddRange(l4s);
            models.AddRange(waterBodies);

            return models;
        }
    }
}
