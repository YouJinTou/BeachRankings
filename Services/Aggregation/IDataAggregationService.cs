using System.Collections.Generic;

namespace BeachRankings.Services.Aggregation
{
    public interface IDataAggregationService
    {
        ICollection<RankContainer> CalculateBeachRanks(int beachId);
    }
}
