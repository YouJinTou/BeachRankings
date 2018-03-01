namespace Services.Aggregation.RankCalculators
{
    using BeachRankings.Models.Enums;
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Aggregation;
    using BeachRankings.Services.Aggregation.RankCalculators;
    using BeachRankings.Services.Aggregation.RankCalculators.Strategies;
    using System.Collections.Generic;

    internal class BestBeachRankCalculator : RankCalculator
    {
        public BestBeachRankCalculator(
            ICollection<IBeachAggregatable> worldBeaches,
            ICollection<IBeachAggregatable> continentBeaches,
            ICollection<IBeachAggregatable> countryBeaches,
            ICollection<IBeachAggregatable> areaBeaches,
            ICollection<IBeachAggregatable> waterBodyBeaches)
            : base (worldBeaches, continentBeaches, countryBeaches, areaBeaches, waterBodyBeaches)
        {
        }

        public override RankContainer CalculateBeachRanks(IBeachAggregatable comparee)
        {
            var strategy = new BestBeachCalculationStrategy();

            return new RankContainer
            {
                ContinentId = comparee.ContinentId,
                CountryId = comparee.CountryId,
                PrimaryDivisionId = comparee.PrimaryDivisionId,
                WaterBodyId = comparee.WaterBodyId,
                FilterType = BeachFilterType.BestBeach,
                FilterDescriptors = this.descriptor.GetFilterDescription(BeachFilterType.BestBeach),
                RankBy = "Best beach",
                WorldRank = this.GetWorldRank(comparee, strategy),
                ContinentRank = this.GetContinentRank(comparee, strategy),
                CountryRank = this.GetCountryRank(comparee, strategy),
                AreaRank = this.GetAreaRank(comparee, strategy),
                WaterBodyRank = this.GetWaterBodyRank(comparee, strategy)
            };
        }
    }
}
