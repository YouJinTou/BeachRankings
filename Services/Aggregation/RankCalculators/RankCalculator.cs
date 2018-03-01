namespace BeachRankings.Services.Aggregation.RankCalculators
{
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Aggregation.RankCalculators.Strategies;
    using System.Collections.Generic;
    
    internal abstract class RankCalculator
    {
        protected ICollection<IBeachAggregatable> worldBeaches;
        protected ICollection<IBeachAggregatable> continentBeaches;
        protected ICollection<IBeachAggregatable> countryBeaches;
        protected ICollection<IBeachAggregatable> areaBeaches;
        protected ICollection<IBeachAggregatable> waterBodyBeaches;
        protected BeachFilterDescriptor descriptor;

        public RankCalculator(
            ICollection<IBeachAggregatable> worldBeaches,
            ICollection<IBeachAggregatable> continentBeaches,
            ICollection<IBeachAggregatable> countryBeaches,
            ICollection<IBeachAggregatable> areaBeaches,
            ICollection<IBeachAggregatable> waterBodyBeaches)
        {
            this.worldBeaches = worldBeaches;
            this.continentBeaches = continentBeaches;
            this.countryBeaches = countryBeaches;
            this.areaBeaches = areaBeaches;
            this.waterBodyBeaches = waterBodyBeaches;
            this.descriptor = new BeachFilterDescriptor();
        }

        public abstract RankContainer CalculateBeachRanks(IBeachAggregatable comparee);

        protected int GetWorldRank(
            IBeachAggregatable comparee, IRankCalculationStrategy strategy)
        {
            return strategy.CalculateRank(comparee, this.worldBeaches);
        }

        protected int GetContinentRank(
            IBeachAggregatable comparee, IRankCalculationStrategy strategy)
        {
            return strategy.CalculateRank(comparee, this.continentBeaches);
        }

        protected int GetCountryRank(
            IBeachAggregatable comparee, IRankCalculationStrategy strategy)
        {
            return strategy.CalculateRank(comparee, this.countryBeaches);
        }

        protected int GetAreaRank(
            IBeachAggregatable comparee, IRankCalculationStrategy strategy)
        {
            return strategy.CalculateRank(comparee, this.areaBeaches);
        }

        protected int GetWaterBodyRank(
            IBeachAggregatable comparee, IRankCalculationStrategy strategy)
        {
            return strategy.CalculateRank(comparee, this.waterBodyBeaches);
        }
    }
}
