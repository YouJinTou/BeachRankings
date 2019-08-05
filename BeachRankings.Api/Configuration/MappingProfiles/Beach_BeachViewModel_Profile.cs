using AutoMapper;
using BeachRankings.Api.Models.Beaches;
using BeachRankings.Core.Models;

namespace BeachRankings.Api.Configuration.MappingProfiles
{
    public class Beach_BeachViewModel_Profile : Profile
    {
        public Beach_BeachViewModel_Profile()
        {
            this.CreateMap<Beach, BeachViewModel>();
        }
    }
}
