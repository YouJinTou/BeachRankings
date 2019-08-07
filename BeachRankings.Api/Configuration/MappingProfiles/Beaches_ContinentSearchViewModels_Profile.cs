using AutoMapper;
using BeachRankings.Api.Models.Beaches;
using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BeachRankings.Api.Configuration.MappingProfiles
{
    public class Beaches_ContinentSearchViewModels_Profile : Profile
    {
        public Beaches_ContinentSearchViewModels_Profile()
        {
            var map = this.CreateMap<IEnumerable<Beach>, IEnumerable<ContinentSearchViewModel>>();

            map.ConstructUsing((beaches, ctx) =>
            {
                var prefix = ctx.Items["prefix"].ToString();
                var models = beaches
                    .Where(b => b.Continent.ToLower().StartsWith(prefix))
                    .GroupBy(b => b.Continent)
                    .Select(g => new ContinentSearchViewModel
                    {
                        BeachesCount = g.Count(),
                        Name = g.Key
                    });

                return models;
            });
        }
    }
}
