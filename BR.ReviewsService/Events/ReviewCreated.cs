using BR.ReviewsService.Models;
using BR.Core.Events;
using Newtonsoft.Json;

namespace BR.ReviewsService.Events
{
    public class ReviewCreated : EventBase
    {
        public ReviewCreated(Review review)
            : base(review.Id.ToString(), 0, JsonConvert.SerializeObject(review))
        {
        }
    }
}
