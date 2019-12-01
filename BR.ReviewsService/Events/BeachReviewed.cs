using BR.Core.Events;
using BR.ReviewsService.Models;
using Newtonsoft.Json;

namespace BR.ReviewsService.Events
{
    public class BeachReviewed : EventBase
    {
        public BeachReviewed(string beachId, int offset, BeachReviewedModel model)
            : base(beachId, offset, JsonConvert.SerializeObject(model))
        {

        }
    }
}
