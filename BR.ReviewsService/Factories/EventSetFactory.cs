using BR.Core.Events;
using BR.Core.Tools;
using BR.ReviewsService.Events;
using BR.ReviewsService.Models;

namespace BR.ReviewsService.Factories
{
    public static class EventSetFactory
    {
        public static EventSet CreateSet(
            Review review, EventStream beachReviewedStream, EventStream userLeftReviewStream)
        {
            Validator.ThrowIfAnyNull(review, beachReviewedStream, userLeftReviewStream);

            var reviewCreated = new ReviewCreated(review);
            var beachReviwedModel = new BeachReviewedModel(
                review.BeachId,
                beachReviewedStream.GetNextOffset(nameof(BeachReviewed)),
                review.UserId,
                review.Id);
            var beachReviewed = new BeachReviewed(beachReviwedModel);
            var userLeftReviewModel = new UserLeftReviewModel(
                review.UserId,
                userLeftReviewStream.GetNextOffset(nameof(UserLeftReview)),
                review.Id,
                review.BeachId);
            var userLeftReview = new UserLeftReview(userLeftReviewModel);
            var set = new EventSet(reviewCreated, beachReviewed, userLeftReview);

            return set;
        }
    }
}
