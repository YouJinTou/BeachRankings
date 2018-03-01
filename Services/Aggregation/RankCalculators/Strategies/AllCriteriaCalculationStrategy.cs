namespace BeachRankings.Services.Aggregation.RankCalculators.Strategies
{
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    internal class AllCriteriaCalculationStrategy : CalculationStrategy
    {
        public override int CalculateRank(
            IBeachAggregatable comparee, ICollection<IBeachAggregatable> beaches)
        {
            if (comparee == null || beaches == null || beaches.Count == 0)
            {
                return NOT_FOUND;
            }

            var sortedBeachRanks = beaches.Select(cb => new RankDto
            {
                BeachId = cb.Id,
                Score = cb.TotalScore
            })
            .Where(r => r.Score != null)
            .OrderByDescending(r => r.Score)
            .ToList();

            return this.GetRank(comparee, sortedBeachRanks);
        }
    }
}
