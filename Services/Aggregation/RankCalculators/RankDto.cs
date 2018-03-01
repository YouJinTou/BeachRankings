namespace BeachRankings.Services.Aggregation.RankCalculators
{
    using System;

    public class RankDto : IEquatable<RankDto>
    {
        public int BeachId { get; set; }

        public double? Score { get; set; }

        public bool Equals(RankDto other)
        {
            return this.BeachId == other.BeachId;
        }
    }
}
