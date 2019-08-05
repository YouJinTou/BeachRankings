using AutoMapper;
using BeachRankings.Api.Models.Beaches;
using BeachRankings.Core.Models;

namespace BeachRankings.Api.Configuration.MappingProfiles
{
    public class AddBeachModel_Beach_Profile : Profile
    {
        public AddBeachModel_Beach_Profile()
        {
            this.CreateMap<AddBeachModel, Beach>();
        }
    }
}
