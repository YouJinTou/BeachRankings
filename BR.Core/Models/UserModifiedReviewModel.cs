using System;

namespace BR.Core.Models
{
    public class UserModifiedReviewModel
    {
        public string UserId { get; set; }

        public Guid ReviewId { get; set; }

        public string BeachId { get; set; }
    }
}
