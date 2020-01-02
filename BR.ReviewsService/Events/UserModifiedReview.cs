using BR.Core.Models;
using BR.ReviewsService.Models;

namespace BR.ReviewsService.Events
{
    public class UserModifiedReview : EventBase
    {
        public UserModifiedReview(UserModifiedReviewModel model)
            : base(model.UserId, model.Offset, model)
        {
        }
    }
}
