﻿using AutoMapper;
using BeachRankings.Api.Abstractions;
using BeachRankings.Api.Models.Beaches;
using BeachRankings.Core.Abstractions;
using BeachRankings.Core.Factories;
using BeachRankings.Core.Models;
using BeachRankings.Core.Tools;
using System.Threading.Tasks;

namespace BeachRankings.Api.Services
{
    public class BeachService : IBeachService
    {
        private readonly IMapper mapper;
        private readonly IBeachesRepository beaches;

        public BeachService(IMapper mapper, IBeachesRepository beaches)
        {
            this.mapper = mapper;
            this.beaches = beaches;
        }

        public async Task<BeachViewModel> GetAsync(string id)
        {
            InputValidator.ThrowIfNullOrWhiteSpace(id);

            var primaryKey = DalObjectsFactory.CreatePrimaryKey(
                Beach.PrimaryPartitionKeyType.ToString(), id);
            var beach = await this.beaches.GetAsync(primaryKey);

            return this.mapper.Map<BeachViewModel>(beach);
        }

        public async Task AddAsync(AddBeachModel addBeachModel)
        {
            InputValidator.ThrowIfNull(addBeachModel);

            var beach = this.mapper.Map<Beach>(addBeachModel);

            await beaches.AddAsync(beach);
        }
    }
}