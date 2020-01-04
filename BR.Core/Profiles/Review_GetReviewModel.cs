using AutoMapper;
using BR.Core.Models;

namespace BR.Core.Profiles
{
    public class Review_GetReviewModel : Profile
    {
        public Review_GetReviewModel()
        {
            this.CreateMap<Review, GetReviewModel>();
        }
    }
}
