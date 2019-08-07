using AutoMapper;
using BeachRankings.Api.Models.Beaches;
using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BeachRankings.Api.Configuration.MappingProfiles
{
    public class Beaches_CountrySearchViewModels_Profile : Profile
    {
        public Beaches_CountrySearchViewModels_Profile()
        {
            var map = this.CreateMap<IEnumerable<Beach>, IEnumerable<CountrySearchViewModel>>();

            map.ConstructUsing((beaches, ctx) =>
            {
                var prefix = ctx.Items["prefix"].ToString();
                var models = beaches
                    .Where(b => b.Country?.ToLower()?.StartsWith(prefix) ?? false)
                    .GroupBy(b => b.Country)
                    .Select(g => new CountrySearchViewModel
                    {
                        BeachesCount = g.Count(),
                        ContinentName = beaches.FirstOrDefault()?.Continent,
                        Name = g.Key
                    });

                return models;
            });
        }
    }
}
