using BR.Core.Extensions;
using BR.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BR.Seed.Extensions
{
    public static class SeedExtensions
    {
        public static Place GetContinent(
            this SeedContinent continent,
            IEnumerable<string> waterBodies)
        {
            var place = new Place
            {
                Id = continent.name,
                Name = continent.name,
                Type = PlaceType.Continent.ToString(),
                WaterBodies = waterBodies.NonNullOrEmpty(),
                Children = continent.Country.Select(c => c.name)
            };

            return place;
        }

        public static IEnumerable<Place> GetCountry(
            this SeedContinentCountry country,
            SeedContinent continent,
            IEnumerable<string> waterBodies)
        {
            var place1 = new Place
            {
                Id = $"{continent.name}_{country.name}",
                Name = country.name,
                Type = PlaceType.Country.ToString(),
                Continent = continent.name,
                WaterBodies = waterBodies.NonNullOrEmpty(),
                Children = country.L1?.Select(l => l.name)
            };
            var place2 = new Place
            {
                Id = country.name,
                Name = country.name,
                Type = PlaceType.Country.ToString(),
                Continent = continent.name,
                WaterBodies = waterBodies.NonNullOrEmpty(),
                Children = country.L1?.Select(l => l.name)
            };

            return new[] { place1, place2 };
        }

        public static Place GetL1(
            this SeedContinentCountryL1 l1,
            SeedContinentCountry country,
            SeedContinent continent,
            IEnumerable<string> waterBodies)
        {
            var place = new Place
            {
                Id = $"{country.name}_{l1.name}",
                Name = l1.name,
                Type = PlaceType.L1.ToString(),
                Continent = continent.name,
                Country = country.name,
                WaterBodies = waterBodies.NonNullOrEmpty(),
                Children = l1.L2?.Select(l => l.name)
            };

            return place;
        }

        public static Place GetL2(
            this SeedContinentCountryL1L2 l2,
            SeedContinentCountryL1 l1,
            SeedContinentCountry country,
            SeedContinent continent,
            IEnumerable<string> waterBodies)
        {
            var place = new Place
            {
                Id = $"{country.name}_{l1.name}_{l2.name}",
                Name = l2.name,
                Type = PlaceType.L2.ToString(),
                Continent = continent.name,
                Country = country.name,
                L1 = l1.name,
                WaterBodies = waterBodies.NonNullOrEmpty(),
                Children = l2.L3?.Select(l => l.name)
            };

            return place;
        }

        public static Place GetL3(
            this SeedContinentCountryL1L2L3 l3,
            SeedContinentCountryL1L2 l2,
            SeedContinentCountryL1 l1,
            SeedContinentCountry country,
            SeedContinent continent,
            IEnumerable<string> waterBodies)
        {
            var place = new Place
            {
                Id = $"{country.name}_{l1.name}_{l2.name}_{l3.name}",
                Name = l3.name,
                Type = PlaceType.L3.ToString(),
                Continent = continent.name,
                Country = country.name,
                L1 = l1.name,
                L2 = l2.name,
                WaterBodies = waterBodies.NonNullOrEmpty(),
                Children = l3.L4?.Select(l => l.name)
            };

            return place;
        }

        public static Place GetL4(
            this SeedContinentCountryL1L2L3L4 l4, 
            SeedContinentCountryL1L2L3 l3,
            SeedContinentCountryL1L2 l2,
            SeedContinentCountryL1 l1,
            SeedContinentCountry country,
            SeedContinent continent,
            IEnumerable<string> waterBodies)
        {
            var place = new Place
            {
                Id = $"{country.name}_{l1.name}_{l2.name}_{l3.name}_{l4.name}",
                Name = l4.name,
                Type = PlaceType.L4.ToString(),
                Continent = continent.name,
                Country = country.name,
                L1 = l1.name,
                L2 = l2.name,
                L3 = l3.name,
                WaterBodies = waterBodies.NonNullOrEmpty()
            };

            return place;
        }
    }
}
