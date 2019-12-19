using System;

namespace BR.ReviewsService.Models
{
    public class BeachReviewChangedModel
    {
        public string BeachId { get; set; }

        public Guid ReviewId { get; set; }

        public string UserId { get; set; }

        public int Offset { get; set; }
    }
}
