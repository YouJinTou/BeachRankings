using AutoMapper;
using BR.Core.Extensions;
using BR.Core.Models;
using System;

namespace BR.Core.Profiles
{
    public class ModifyReviewModel_Review : Profile
    {
        public ModifyReviewModel_Review()
        {
            this.CreateMap<ModifyReviewModel, Review>()
                .AfterMap((s, d) => d.LastUpdatedOn = DateTime.UtcNow)
                .AfterMap((s, d) => d.Score = d.CalculateScore());
        }
    }
}
