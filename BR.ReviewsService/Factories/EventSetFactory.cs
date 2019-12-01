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
                beachReviewedStream.NextOffset(nameof(BeachReviewed)),
                review.AuthorId,
                review.Id);
            var beachReviewed = new BeachReviewed(beachReviwedModel);
            var userLeftReviewModel = new UserLeftReviewModel(
                review.AuthorId,
                userLeftReviewStream.NextOffset(nameof(UserLeftReview)),
                review.Id,
                review.BeachId);
            var userLeftReview = new UserLeftReview(userLeftReviewModel);
            var set = new EventSet(reviewCreated, beachReviewed, userLeftReview);

            return set;
        }
    }
}
