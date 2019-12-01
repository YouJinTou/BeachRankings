using BR.Core.Events;
using BR.ReviewsService.Models;

namespace BR.ReviewsService.Events
{
    public class ReviewLeft : EventBase
    {
        public ReviewLeft(ReviewLeftModel model)
            : base(model.UserId, model.Offset, model)
        {
        }
    }
}
