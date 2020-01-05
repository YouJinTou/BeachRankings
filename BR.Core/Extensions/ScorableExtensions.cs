using BR.Core.Abstractions;
using BR.Core.Tools;
using System;
using System.Linq;

namespace BR.Core.Extensions
{
    public static class ScorableExtensions
    {
        public static double? CalculateScore(this IScorable scorable)
        {
            Validator.ThrowIfNull(scorable, "Scorable is empty.");

            var scores = new double?[]
            {
                scorable.SandQuality,
                scorable.BeachCleanliness,
                scorable.BeautifulScenery,
                scorable.CrowdFree,
                scorable.Infrastructure,
                scorable.WaterVisibility,
                scorable.LitterFree,
                scorable.FeetFriendlyBottom,
                scorable.SeaLifeDiversity,
                scorable.CoralReef,
                scorable.Snorkeling,
                scorable.Kayaking,
                scorable.Walking,
                scorable.Camping,
                scorable.LongTermStay
            };

            if (Validator.AllNull(scores))
            {
                return null;
            }

            return (double?)Math.Round((decimal)scores.Average(), 1);
        }
    }
}
