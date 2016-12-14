namespace BeachRankings.Services.Initializers
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;

    public static class GeoInitializer
    {
        private static Lazy<ConcurrentDictionary<int, string>> continents
             = new Lazy<ConcurrentDictionary<int, string>>(() => LoadContinents());
        private static Lazy<ConcurrentDictionary<int, string>> countries
             = new Lazy<ConcurrentDictionary<int, string>>(() => LoadCountries());
        private static Lazy<ConcurrentDictionary<int, string>> waterBodies
             = new Lazy<ConcurrentDictionary<int, string>>(() => LoadWaterBodies());

        public static ConcurrentDictionary<int, string> Continents
        {
            get
            {
                return continents.Value;
            }
        }

        public static ConcurrentDictionary<int, string> Countries
        {
            get
            {
                return countries.Value;
            }
        }

        public static ConcurrentDictionary<int, string> WaterBodies
        {
            get
            {
                return waterBodies.Value;
            }
        }

        private static ConcurrentDictionary<int, string> LoadContinents()
        {
            var continents = new ConcurrentDictionary<int, string>();
            var continentsPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "Continents.txt");

            using (var sr = new StreamReader(continentsPath))
            {
                string continent;
                var id = 1;

                while ((continent = sr.ReadLine()) != null)
                {
                    continents.TryAdd(id, continent);

                    id++;
                }
            }

            return continents;
        }

        private static ConcurrentDictionary<int, string> LoadCountries()
        {
            var countries = new ConcurrentDictionary<int, string>();
            var countriesPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "Countries.txt");

            using (var sr = new StreamReader(countriesPath))
            {
                string country;
                var id = 1;

                while ((country = sr.ReadLine()) != null)
                {
                    countries.TryAdd(id, country);

                    id++;
                }
            }

            return countries;
        }

        private static ConcurrentDictionary<int, string> LoadWaterBodies()
        {
            var waterBodies = new ConcurrentDictionary<int, string>();
            var waterBodiesPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "WaterBodies.txt");

            using (var sr = new StreamReader(waterBodiesPath))
            {
                string waterBody;
                var id = 1;

                while ((waterBody = sr.ReadLine()) != null)
                {
                    waterBodies.TryAdd(id, waterBody);

                    id++;
                }
            }

            return waterBodies;
        }
    }
}