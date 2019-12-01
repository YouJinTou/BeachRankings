using BR.Core.Tools;
using System;

namespace BR.ReviewsService.Models
{
    public class UserLeftReviewModel
    {
        public UserLeftReviewModel(string userId, int offset, Guid reviewId, string beachId)
        {
            this.UserId = Validator.ReturnOrThrowIfNullOrWhiteSpace(userId);
            this.Offset = offset;
            this.ReviewId = Validator.ReturnOrThrowIfNullOrWhiteSpace(reviewId);
            this.BeachId = Validator.ReturnOrThrowIfNullOrWhiteSpace(beachId);
        }

        public string UserId { get; set; }

        public int Offset { get; set; }

        public Guid ReviewId { get; }

        public string BeachId { get; }
    }
}
