using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BeachRankings.Loader.Models
{
    public partial class Seed
    {
        public IEnumerable<Location> GetLocations()
        {
            var locations = new List<Location>();
            var currentLocation = string.Empty;

            foreach (var continent in this.Continent)
            {
                locations.Add(this.GetLocation(continent.name));

                foreach (var country in continent.Country ?? new SeedContinentCountry[] { })
                {
                    locations.Add(this.GetLocation(continent.name, country.name, waterBody: country.waterBody));

                    foreach (var l1 in country.L1 ?? new SeedContinentCountryL1[] { })
                    {
                        locations.Add(this.GetLocation(continent.name, country.name, l1.name, waterBody: this.GetWaterBody(country.waterBody, l1.waterBody)));

                        foreach (var l2 in l1.L2 ?? new SeedContinentCountryL1L2[] { })
                        {
                            locations.Add(this.GetLocation(continent.name, country.name, l1.name, l2.name, waterBody: this.GetWaterBody(country.waterBody, l1.waterBody, l2.waterBody)));

                            foreach (var l3 in l2.L3 ?? new SeedContinentCountryL1L2L3[] { })
                            {
                                locations.Add(this.GetLocation(continent.name, country.name, l1.name, l2.name, l3.name, waterBody: this.GetWaterBody(country.waterBody, l1.waterBody, l2.waterBody, l3.waterBody)));

                                foreach (var l4 in l3.L4 ?? new SeedContinentCountryL1L2L3L4[] { })
                                {
                                    locations.Add(this.GetLocation(continent.name, country.name, l1.name, l2.name, l3.name, l4.name, this.GetWaterBody(country.waterBody, l1.waterBody, l2.waterBody, l3.waterBody, l4.waterBody)));
                                }
                            }
                        }
                    }
                }
            }

            return locations;
        }

        private string GetWaterBody(params string[] waterBodies)
        {
            return waterBodies.LastOrDefault(wb => !string.IsNullOrWhiteSpace(wb));
        }

        private Location GetLocation(
            string continent, 
            string country = null, 
            string l1 = null, 
            string l2 = null, 
            string l3 = null, 
            string l4 = null, 
            string waterBody = null)
        {
            return new Location(continent, country, l1, l2, l3, l4, waterBody);
        }
    }
}
