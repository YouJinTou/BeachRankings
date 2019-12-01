using BR.Core.Events;
using BR.ReviewsService.Models;
using Newtonsoft.Json;

namespace BR.ReviewsService.Events
{
    public class BeachReviewed : EventBase
    {
        public BeachReviewed(BeachReviewedModel model)
            : base(model.BeachId, model.Offset, JsonConvert.SerializeObject(model))
        {
        }
    }
}
