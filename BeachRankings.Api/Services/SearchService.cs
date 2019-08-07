using AutoMapper;
using BeachRankings.Api.Abstractions;
using BeachRankings.Core.Abstractions;
using BeachRankings.Core.Models;
using BeachRankings.Core.Tools;
using System.Collections.Generic;
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

        public async Task<IEnumerable<BeachDbResultModel>> FindBeachesAsync(BeachQueryModel model)
        {
            InputValidator.ThrowIfNull(model);

            var beaches = await this.beachesRepository.GetManyAsync(model);
            var models = this.mapper.Map<IEnumerable<BeachDbResultModel>>(beaches);

            return models;
        }
    }
}
