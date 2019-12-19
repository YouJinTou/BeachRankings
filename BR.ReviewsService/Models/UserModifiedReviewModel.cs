using System;

namespace BR.ReviewsService.Models
{
    public class UserModifiedReviewModel
    {
        public string UserId { get; set; }

        public Guid ReviewId { get; set; }

        public string BeachId { get; set; }

        public int Offset { get; set; }
    }
}
