using BR.Core.Tools;
using System;

namespace BR.ReviewsService.Models
{
    public class BeachReviewedModel
    {
        public BeachReviewedModel(string beachId, int offset, string authorId, Guid reviewId)
        {
            this.BeachId = Validator.ReturnOrThrowIfNullOrWhiteSpace(beachId);
            this.Offset = offset;
            this.AuthorId = Validator.ReturnOrThrowIfNullOrWhiteSpace(authorId);
            this.ReviewId = reviewId;
        }

        public string BeachId { get; }

        public int Offset { get; }

        public string AuthorId { get; }

        public Guid ReviewId { get; }
    }
}
