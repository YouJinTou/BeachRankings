using AutoMapper;
using BeachRankings.Api.Models.Beaches;
using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BeachRankings.Api.Configuration.MappingProfiles
{
    public class Beaches_WaterBodySearchViewModels_Profile : Profile
    {
        public Beaches_WaterBodySearchViewModels_Profile()
        {
            var map = this.CreateMap<IEnumerable<Beach>, IEnumerable<WaterBodySearchViewModel>>();

            map.ConstructUsing((beaches, ctx) =>
            {
                var prefix = ctx.Items["prefix"].ToString();
                var models = beaches
                    .Where(b => b.WaterBody.ToLower().StartsWith(prefix))
                    .GroupBy(b => b.WaterBody)
                    .Select(g => new WaterBodySearchViewModel
                    {
                        BeachesCount = g.Count(),
                        Name = g.Key
                    });

                return models;
            });
        }
    }
}
