using AutoMapper;
using BeachRankings.Api.Models.Beaches;
using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BeachRankings.Api.Configuration.MappingProfiles
{
    public class Beaches_L2SearchViewModels_Profile : Profile
    {
        public Beaches_L2SearchViewModels_Profile()
        {
            var map = this.CreateMap<IEnumerable<Beach>, IEnumerable<L2SearchViewModel>>();

            map.ConstructUsing((beaches, ctx) =>
            {
                var prefix = ctx.Items["prefix"].ToString();
                var models = beaches
                    .Where(b => b.L2?.ToLower()?.StartsWith(prefix) ?? false)
                    .GroupBy(b => b.L2)
                    .Select(g => new L2SearchViewModel
                    {
                        BeachesCount = g.Count(),
                        CountryName = beaches.FirstOrDefault()?.Country,
                        Name = g.Key
                    });

                return models;
            });
        }
    }
}
