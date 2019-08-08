using AutoMapper;
using BeachRankings.Api.Models.Beaches;
using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BeachRankings.Api.Configuration.MappingProfiles
{
    public class Beaches_L3SearchViewModels_Profile : Profile
    {
        public Beaches_L3SearchViewModels_Profile()
        {
            var map = this.CreateMap<IEnumerable<Beach>, IEnumerable<L3SearchViewModel>>();

            map.ConstructUsing((beaches, ctx) =>
            {
                var prefix = ctx.Items["prefix"].ToString();
                var models = beaches
                    .Where(b => b.L3?.ToLower()?.StartsWith(prefix) ?? false)
                    .GroupBy(b => b.L3)
                    .Select(g => new L3SearchViewModel
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
