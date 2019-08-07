using AutoMapper;
using BeachRankings.Core.Models;

namespace BeachRankings.Core.Configuration.MapperProfiles
{
    public class Beach_BeachDbResultModel_Profile : Profile
    {
        public Beach_BeachDbResultModel_Profile()
        {
            var map = this.CreateMap<Beach, BeachDbResultModel>();
        }
    }
}
