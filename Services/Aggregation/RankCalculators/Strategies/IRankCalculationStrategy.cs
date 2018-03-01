namespace BeachRankings.Services.Aggregation.RankCalculators.Strategies
{
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;

    internal interface IRankCalculationStrategy
    {
        int CalculateRank(IBeachAggregatable comparee, ICollection<IBeachAggregatable> beaches);
    }
}
