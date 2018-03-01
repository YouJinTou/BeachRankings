using BeachRankings.Models;
using System.Collections.Generic;

namespace Tests.Models
{
    internal class BeachTestable : Beach
    {
        public void SetReviews(ICollection<Review> reviews)
        {
            this.Reviews = reviews;
        }
    }
}
