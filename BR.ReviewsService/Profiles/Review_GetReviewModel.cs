using AutoMapper;
using BR.ReviewsService.Models;

namespace BR.ReviewsService.Profiles
{
    public class Review_GetReviewModel : Profile
    {
        public Review_GetReviewModel()
        {
            this.CreateMap<Review, GetReviewModel>();
        }
    }
}
