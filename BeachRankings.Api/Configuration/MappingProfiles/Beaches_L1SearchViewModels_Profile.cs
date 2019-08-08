using AutoMapper;
using BeachRankings.Api.Models.Beaches;
using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BeachRankings.Api.Configuration.MappingProfiles
{
    public class Beaches_L1SearchViewModels_Profile : Profile
    {
        public Beaches_L1SearchViewModels_Profile()
        {
            var map = this.CreateMap<IEnumerable<Beach>, IEnumerable<L1SearchViewModel>>();

            map.ConstructUsing((beaches, ctx) =>
            {
                var prefix = ctx.Items["prefix"].ToString();
                var models = beaches
                    .Where(b => b.L1?.ToLower()?.StartsWith(prefix) ?? false)
                    .GroupBy(b => b.L1)
                    .Select(g => new L1SearchViewModel
                    {
                        BeachesCount = g.Count(),
                        CountryName = g.FirstOrDefault()?.Country,
                        Name = g.Key
                    });

                return models;
            });
        }
    }
}
