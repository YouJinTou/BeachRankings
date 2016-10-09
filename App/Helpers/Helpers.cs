namespace App.Helpers
{
    using BeachRankings.Models.Misc.Interfaces;
    using System;
    using System.Linq;
    public static class BeachesHelper
    {

    }

    public static class ReviewsHelper
    {
        private static readonly int BeachCriteriaCount = 12;

        public static double? GetTotalReviewScore(IRateable review)
        {
            double score = 0;
            int nullCount = 0; // Count of criteria NOT voted for

            score += (review.WaterQuality ?? 0);
            nullCount += ((review.WaterQuality == null) ? 1 : 0);

            score += (review.SeafloorCleanliness ?? 0);
            nullCount += ((review.SeafloorCleanliness == null) ? 1 : 0);

            score += (review.CoralReefFactor ?? 0);
            nullCount += ((review.CoralReefFactor == null) ? 1 : 0);

            score += (review.SeaLifeDiversity ?? 0);
            nullCount += ((review.SeaLifeDiversity == null) ? 1 : 0);

            score += (review.SnorkelingSuitability ?? 0);
            nullCount += ((review.SnorkelingSuitability == null) ? 1 : 0);

            score += (review.BeachCleanliness ?? 0);
            nullCount += ((review.BeachCleanliness == null) ? 1 : 0);

            score += (review.CrowdFreeFactor ?? 0);
            nullCount += ((review.CrowdFreeFactor == null) ? 1 : 0);

            score += (review.SandQuality ?? 0);
            nullCount += ((review.SandQuality == null) ? 1 : 0);

            score += (review.BreathtakingEnvironment ?? 0);
            nullCount += ((review.BreathtakingEnvironment == null) ? 1 : 0);

            score += (review.TentSuitability ?? 0);
            nullCount += ((review.TentSuitability == null) ? 1 : 0);

            score += (review.KayakSuitability ?? 0);
            nullCount += ((review.KayakSuitability == null) ? 1 : 0);

            score += (review.LongStaySuitability ?? 0);
            nullCount += ((review.LongStaySuitability == null) ? 1 : 0);

            double? result = null;

            if (nullCount != BeachCriteriaCount)
            {
                result = Math.Round((double)(score / (BeachCriteriaCount - nullCount)), 1);
            }

            return result;
        }
    }
}