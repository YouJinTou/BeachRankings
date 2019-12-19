using BR.Core.Abstractions;
using BR.Core.Tools;
using System;
using System.Linq;

namespace BR.ReviewsService.Models
{
    public class Review : IAggregate
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public string BeachId { get; set; }

        public string UserId { get; set; }

        public DateTime AddedOn { get; set; }

        public DateTime LastUpdatedOn { get; set; }

        public double? Score { get; set; }

        public double? SandQuality { get; set; }

        public double? BeachCleanliness { get; set; }

        public double? BeautifulScenery { get; set; }

        public double? CrowdFree { get; set; }

        public double? Infrastructure { get; set; }

        public double? WaterVisibility { get; set; }

        public double? LitterFree { get; set; }

        public double? FeetFriendlyBottom { get; set; }

        public double? SeaLifeDiversity { get; set; }

        public double? CoralReef { get; set; }

        public double? Snorkeling { get; set; }

        public double? Kayaking { get; set; }

        public double? Walking { get; set; }

        public double? Camping { get; set; }

        public double? LongTermStay { get; set; }

        public static double? CalculateScore(Review review)
        {
            Validator.ThrowIfNull(review, "Review is empty.");

            var scores = new double?[]
            {
                review.SandQuality,
                review.BeachCleanliness,
                review.BeautifulScenery,
                review.CrowdFree,
                review.Infrastructure,
                review.WaterVisibility,
                review.LitterFree,
                review.FeetFriendlyBottom,
                review.SeaLifeDiversity,
                review.CoralReef,
                review.Snorkeling,
                review.Kayaking,
                review.Walking,
                review.Camping,
                review.LongTermStay
            };

            if (Validator.AllNull(scores))
            {
                return null;
            }

            return (double?)Math.Round((decimal)scores.Average(), 1);
        }

        public static Review CreateNull()
        {
            return new Review();
        }
    }
}
