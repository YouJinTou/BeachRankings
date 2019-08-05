using AutoMapper;
using BeachRankings.Api.Models.Reviews;
using BeachRankings.Core.Models;

namespace BeachRankings.Api.Configuration.MappingProfiles
{
    public class AddReviewModel_Review_Profile : Profile
    {
        public AddReviewModel_Review_Profile()
        {
            this.CreateMap<AddReviewModel, Review>();
        }
    }
}
