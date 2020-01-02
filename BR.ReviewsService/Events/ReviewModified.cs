using BR.Core.Models;
using BR.ReviewsService.Models;

namespace BR.ReviewsService.Events
{
    public class ReviewModified : EventBase
    {
        public ReviewModified(ModifyReviewModel model, int offset)
            : base(model.Id.ToString(), offset, model)
        {
        }
    }
}
