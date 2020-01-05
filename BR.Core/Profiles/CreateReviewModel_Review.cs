using AutoMapper;
using BR.Core.Extensions;
using BR.Core.Models;
using System;

namespace BR.Core.Profiles
{
    public class CreateReviewModel_Review : Profile
    {
        public CreateReviewModel_Review()
        {
            this.CreateMap<CreateReviewModel, Review>()
                .AfterMap((s, d) => d.Id = Guid.NewGuid())
                .AfterMap((s, d) => d.AddedOn = DateTime.UtcNow)
                .AfterMap((s, d) => d.Score = d.CalculateScore());
        }
    }
}
