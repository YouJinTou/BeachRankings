using BR.Core.Events;
using BR.ReviewsService.Models;

namespace BR.ReviewsService.Events
{
    public class BeachReviewChanged : EventBase
    {
        public BeachReviewChanged(BeachReviewChangedModel model)
            : base(model.BeachId, model.Offset, model)
        {
        }
    }
}
