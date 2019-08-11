using AutoMapper;
using BeachRankings.Api.Models.Beaches;
using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BeachRankings.Api.Configuration.MappingProfiles
{
    public class Beaches_BeachSearchViewModels_Profile : Profile
    {
        public Beaches_BeachSearchViewModels_Profile()
        {
            var map = this.CreateMap<IEnumerable<Beach>, IEnumerable<BeachSearchViewModel>>();

            map.ConstructUsing((beaches, ctx) =>
            {
                var prefix = ctx.Items["prefix"].ToString();
                var models = beaches
                    .Where(b => b.Name.ToLower().StartsWith(prefix))
                    .Select(b => new BeachSearchViewModel
                    {
                        Name = b.Name,
                        Country = b.Country,
                        Id = b.Location,
                        LastLevel = b.GetLastLevel()
                    });

                return models;
            });
        }
    }
}
