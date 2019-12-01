using BR.Core.Events;
using BR.Core.Tools;
using BR.ReviewsService.Events;

namespace BR.ReviewsService.Models
{
    public class EventSet
    {
        public EventSet(
            ReviewCreated reviewCreated, BeachReviewed beachReviewed, UserLeftReview userLeftReview)
        {
            this.ReviewCreated = Validator.ReturnOrThrowIfNullOrWhiteSpace(reviewCreated);
            this.BeachReviewed = Validator.ReturnOrThrowIfNullOrWhiteSpace(beachReviewed);
            this.UserLeftReview = Validator.ReturnOrThrowIfNullOrWhiteSpace(userLeftReview);
        }

        public ReviewCreated ReviewCreated { get; }

        public BeachReviewed BeachReviewed { get; }

        public UserLeftReview UserLeftReview { get; }

        public EventStream ToStream()
        {
            return EventStream.CreateStream(
                this.ReviewCreated, this.BeachReviewed, this.UserLeftReview);
        }
    }
}
