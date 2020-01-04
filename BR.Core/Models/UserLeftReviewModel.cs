using BR.Core.Tools;
using System;

namespace BR.Core.Models
{
    public class UserLeftReviewModel
    {
        public UserLeftReviewModel(string userId, Guid reviewId, string beachId)
        {
            this.UserId = Validator.ReturnOrThrowIfNullOrWhiteSpace(userId);
            this.ReviewId = Validator.ReturnOrThrowIfNullOrWhiteSpace(reviewId);
            this.BeachId = Validator.ReturnOrThrowIfNullOrWhiteSpace(beachId);
        }

        public string UserId { get; set; }

        public Guid ReviewId { get; }

        public string BeachId { get; }
    }
}
