using BR.Core.Tools;
using System;

namespace BR.ReviewsService.Models
{
    public class BeachReviewedModel
    {
        public BeachReviewedModel(string beachId, int offset, string userId, Guid reviewId)
        {
            this.BeachId = Validator.ReturnOrThrowIfNullOrWhiteSpace(beachId);
            this.Offset = offset;
            this.UserId = Validator.ReturnOrThrowIfNullOrWhiteSpace(userId);
            this.ReviewId = reviewId;
        }

        public string BeachId { get; }

        public int Offset { get; }

        public string UserId { get; }

        public Guid ReviewId { get; }
    }
}
