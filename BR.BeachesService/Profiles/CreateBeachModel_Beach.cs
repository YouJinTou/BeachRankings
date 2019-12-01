using AutoMapper;
using BR.BeachesService.Models;
using System;

namespace BR.BeachesService.Profiles
{
    public class CreateBeachModel_Beach : Profile
    {
        public CreateBeachModel_Beach()
        {
            this.CreateMap<CreateBeachModel, Beach>()
                .AfterMap((s, d) => d.Id = Beach.GetId(d))
                .AfterMap((s, d) => d.AddedOn = DateTime.UtcNow)
                .AfterMap((s, d) => d.Location = Beach.GetId(d))
                .AfterMap((s, d) => d.Score = Beach.CalculateScore(d));
        }
    }
}
