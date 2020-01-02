using BR.Core.Models;
using BR.ReviewsService.Models;

namespace BR.ReviewsService.Events
{
    public class UserLeftReview : EventBase
    {
        public UserLeftReview(UserLeftReviewModel model)
            : base(model.UserId, model.Offset, model)
        {
        }
    }
}
