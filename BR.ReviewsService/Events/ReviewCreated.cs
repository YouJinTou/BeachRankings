using BR.Core.Models;
using BR.ReviewsService.Models;

namespace BR.ReviewsService.Events
{
    public class ReviewCreated : EventBase
    {
        public ReviewCreated(Review review)
            : base(review.Id.ToString(), 0, review)
        {
        }
    }
}
