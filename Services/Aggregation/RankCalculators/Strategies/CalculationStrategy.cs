namespace BeachRankings.Services.Aggregation.RankCalculators.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using BeachRankings.Models.Interfaces;

    internal abstract class CalculationStrategy : IRankCalculationStrategy
    {
        protected const int NOT_FOUND = -1;

        public abstract int CalculateRank(
            IBeachAggregatable comparee, ICollection<IBeachAggregatable> beaches);

        protected int GetRank(
            IBeachAggregatable comparee, IList<RankDto> sortedBeachRanks)
        {
            int zeroBasedRank = sortedBeachRanks
                .IndexOf(new RankDto { BeachId = comparee.Id });

            return (zeroBasedRank == NOT_FOUND) ? NOT_FOUND : (zeroBasedRank + 1);
        }

        protected double? GetCriteriaAverage(params double?[] criteriaValues)
        {
            var nonNullValues = criteriaValues.Where(cv => cv != null && cv != NOT_FOUND);
            int nonNullCount = nonNullValues.Count();
            double? average = nonNullCount == 0 ? null : (nonNullValues.Sum() / nonNullCount);

            return average;
        }
    }
}
