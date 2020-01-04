using BR.Core.Tools;
using System;

namespace BR.Core.Models
{
    public class BeachReviewedModel
    {
        public BeachReviewedModel(string beachId, string userId, Guid reviewId)
        {
            this.BeachId = Validator.ReturnOrThrowIfNullOrWhiteSpace(beachId);
            this.UserId = Validator.ReturnOrThrowIfNullOrWhiteSpace(userId);
            this.ReviewId = reviewId;
        }

        public string BeachId { get; }

        public string UserId { get; }

        public Guid ReviewId { get; }
    }
}
