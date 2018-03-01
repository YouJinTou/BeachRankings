using BeachRankings.Models.Interfaces;
using BeachRankings.Services.Aggregation.RankCalculators;
using BeachRankings.Services.Initializers;
using BeachRankings.Services.Search;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeachRankings.Services.Aggregation
{
    public class DataAggregationService : IDataAggregationService
    {
        private ISearchService searchService;

        public DataAggregationService(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        public ICollection<RankContainer> CalculateBeachRanks(int beachId)
        {
            var comparee = this.searchService.SearchByBeachId(beachId);
            var worldBeaches = this.GetWorldBeaches();
            var continentBeaches = this.searchService.SearchByContinentId(comparee.ContinentId ?? 0);
            var countryBeaches = this.searchService.SearchByCountryId(comparee.CountryId);
            var areaBeaches = this.searchService.SearchByPrimaryDivisionId(comparee.PrimaryDivisionId ?? 0);
            var waterBodyBeaches = this.searchService.SearchByWaterBodyId(comparee.WaterBodyId);
            var rankCalculators = this.GetRankCalculators(
                worldBeaches, continentBeaches, countryBeaches, areaBeaches, waterBodyBeaches);

            return rankCalculators.Select(c => c.CalculateBeachRanks(comparee)).ToList();
        }

        private IEnumerable<RankCalculator> GetRankCalculators(
            ICollection<IBeachAggregatable> worldBeaches,
            ICollection<IBeachAggregatable> continentBeaches,
            ICollection<IBeachAggregatable> countryBeaches,
            ICollection<IBeachAggregatable> areaBeaches,
            ICollection<IBeachAggregatable> waterBodyBeaches)
        {
            var rankCalculatorBaseType = typeof(RankCalculator);
            var rankCalculatorChildrenTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => rankCalculatorBaseType.IsAssignableFrom(p) && !p.IsAbstract);
            var instances = rankCalculatorChildrenTypes
                .Select(r => (RankCalculator)Activator.CreateInstance(
                    r, new[] { worldBeaches, continentBeaches, countryBeaches, areaBeaches, waterBodyBeaches }))
                .ToList();

            return instances;
        }

        private ICollection<IBeachAggregatable> GetWorldBeaches()
        {
            var worldBeaches = new List<IBeachAggregatable>();
            var continentIds = GeoInitializer.Continents.Keys;

            foreach (var id in continentIds)
            {
                worldBeaches.AddRange(this.searchService.SearchByContinentId(id));
            }

            return worldBeaches;
        }
    }
}
