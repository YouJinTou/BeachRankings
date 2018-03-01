namespace BeachRankings.Services.Aggregation.RankCalculators.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using BeachRankings.Models.Interfaces;

    internal class SnorkelingCalculationStrategy : CalculationStrategy
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
                Score = this.GetCriteriaAverage(
                    cb.WaterVisibility,
                    cb.LitterFree,
                    cb.SeaLifeDiversity,
                    cb.CoralReef,
                    cb.Snorkeling)
            })
            .Where(r => r.Score != null)
            .OrderByDescending(r => r.Score)
            .ToList();

            return this.GetRank(comparee, sortedBeachRanks);
        }
    }
}
