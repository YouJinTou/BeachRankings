using BR.Core.Extensions;
using BR.Core.Models;
using BR.Seed.Models;
using System.Collections.Generic;
using System.Linq;

namespace BR.Seed.Extensions
{
    public static class SeedExtensions
    {
        public static string GetId(this SeedContinent continent) 
            => continent.name.Latinize();

        public static string GetId(this SeedContinentCountry country)
            => $"{country.continent}_{country.name}".Latinize();

        public static string GetId(this SeedContinentCountryL1 l1)
            => $"{l1.continent}_{l1.country}_{l1.name}".Latinize();

        public static string GetId(this SeedContinentCountryL1L2 l2)
            => $"{l2.continent}_{l2.country}_{l2.L1}_{l2.name}".Latinize();

        public static string GetId(this SeedContinentCountryL1L2L3 l3)
           => $"{l3.continent}_{l3.country}_{l3.L1}_{l3.L2}_{l3.name}".Latinize();

        public static string GetId(this SeedContinentCountryL1L2L3L4 l4)
            => $"{l4.continent}_{l4.country}_{l4.L1}_{l4.L2}_{l4.L3}_{l4.name}".Latinize();

        public static Place GetContinent(
            this SeedContinent continent, IEnumerable<string> waterBodies)
        {
            var place = new Place
            {
                Id = continent.GetId(),
                Name = continent.name.Latinize(),
                Type = PlaceType.Continent.ToString(),
                WaterBodies = waterBodies.NonNullOrEmpty(),
                Children = continent.Country.Select(c => c.GetId())
            };

            return place;
        }

        public static IEnumerable<Place> GetCountry(
            this SeedContinentCountry country, IEnumerable<string> waterBodies)
        {
            var place1 = new Place
            {
                Id = country.GetId(),
                Name = country.name.Latinize(),
                Type = PlaceType.Country.ToString(),
                Continent = country.continent.Latinize(),
                WaterBodies = waterBodies.NonNullOrEmpty(),
                Children = country.L1?.Select(l => l.GetId())
            };
            var place2 = new Place
            {
                Id = country.name.Latinize(),
                Name = country.name.Latinize(),
                Type = PlaceType.Country.ToString(),
                Continent = country.continent.Latinize(),
                WaterBodies = waterBodies.NonNullOrEmpty(),
                Children = country.L1?.Select(l => l.GetId())
            };

            return new[] { place1, place2 };
        }

        public static Place GetL1(this SeedContinentCountryL1 l1, IEnumerable<string> waterBodies)
        {
            var place = new Place
            {
                Id = l1.GetId(),
                Name = l1.name.Latinize(),
                Type = PlaceType.L1.ToString(),
                Continent = l1.continent.Latinize(),
                Country = l1.country.Latinize(),
                WaterBodies = waterBodies.NonNullOrEmpty(),
                Children = l1.L2?.Select(l => l.GetId())
            };

            return place;
        }

        public static Place GetL2(
            this SeedContinentCountryL1L2 l2, IEnumerable<string> waterBodies)
        {
            var place = new Place
            {
                Id = l2.GetId(),
                Name = l2.name.Latinize(),
                Type = PlaceType.L2.ToString(),
                Continent = l2.continent.Latinize(),
                Country = l2.country.Latinize(),
                L1 = l2.L1.Latinize(),
                WaterBodies = waterBodies.NonNullOrEmpty(),
                Children = l2.L3?.Select(l => l.GetId())
            };

            return place;
        }

        public static Place GetL3(
            this SeedContinentCountryL1L2L3 l3, IEnumerable<string> waterBodies)
        {
            var place = new Place
            {
                Id = l3.GetId(),
                Name = l3.name.Latinize(),
                Type = PlaceType.L3.ToString(),
                Continent = l3.continent.Latinize(),
                Country = l3.country.Latinize(),
                L1 = l3.L1.Latinize(),
                L2 = l3.L2.Latinize(),
                WaterBodies = waterBodies.NonNullOrEmpty(),
                Children = l3.L4?.Select(l => l.GetId())
            };

            return place;
        }

        public static Place GetL4(
            this SeedContinentCountryL1L2L3L4 l4, IEnumerable<string> waterBodies)
        {
            var place = new Place
            {
                Id = l4.GetId(),
                Name = l4.name.Latinize(),
                Type = PlaceType.L4.ToString(),
                Continent = l4.continent.Latinize(),
                Country = l4.country.Latinize(),
                L1 = l4.L1.Latinize(),
                L2 = l4.L2.Latinize(),
                L3 = l4.L3.Latinize(),
                WaterBodies = waterBodies.NonNullOrEmpty()
            };

            return place;
        }
    }
}
