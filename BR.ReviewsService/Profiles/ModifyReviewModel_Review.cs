using AutoMapper;
using BR.ReviewsService.Models;
using System;

namespace BR.ReviewsService.Profiles
{
    public class ModifyReviewModel_Review : Profile
    {
        public ModifyReviewModel_Review()
        {
            this.CreateMap<ModifyReviewModel, Review>()
                .AfterMap((s, d) => d.LastUpdatedOn = DateTime.UtcNow)
                .AfterMap((s, d) => d.Score = Review.CalculateScore(d));
        }
    }
}
