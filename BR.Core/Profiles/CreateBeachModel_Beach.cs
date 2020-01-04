using AutoMapper;
using BR.Core.Models;
using System;

namespace BR.Core.Profiles
{
    public class CreateBeachModel_Beach : Profile
    {
        public CreateBeachModel_Beach()
        {
            this.CreateMap<CreateBeachModel, Beach>()
                .AfterMap((s, d) => d.Id = Beach.GetId(d))
                .AfterMap((s, d) => d.AddedOn = DateTime.UtcNow)
                .AfterMap((s, d) => d.LastUpdatedOn = DateTime.UtcNow)
                .AfterMap((s, d) => d.Location = Beach.GetLocation(d))
                .AfterMap((s, d) => d.Score = Beach.CalculateScore(d));
        }
    }
}
