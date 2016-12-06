namespace BeachRankings.App.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class GeoInitializer
    {
        private static Lazy<Dictionary<int, string>> continents
             = new Lazy<Dictionary<int, string>>(() => LoadContinents());
        private static Lazy<Dictionary<int, string>> countries
             = new Lazy<Dictionary<int, string>>(() => LoadCountries());
        private static Lazy<Dictionary<int, string>> waterBodies
             = new Lazy<Dictionary<int, string>>(() => LoadWaterBodies());

        public static Dictionary<int, string> Continents
        {
            get
            {
                return continents.Value;
            }
        }

        public static Dictionary<int, string> Countries
        {
            get
            {
                return countries.Value;
            }
        }

        public static Dictionary<int, string> WaterBodies
        {
            get
            {
                return waterBodies.Value;
            }
        }

        private static Dictionary<int, string> LoadContinents()
        {
            var continents = new Dictionary<int, string>();
            var continentsPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "Continents.txt");

            using (var sr = new StreamReader(continentsPath))
            {
                string continent;
                var id = 1;

                while ((continent = sr.ReadLine()) != null)
                {
                    continents.Add(id, continent);

                    id++;
                }
            }

            return continents;
        }

        private static Dictionary<int, string> LoadCountries()
        {
            var countries = new Dictionary<int, string>();
            var countriesPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "Countries.txt");

            using (var sr = new StreamReader(countriesPath))
            {
                string country;
                var id = 1;

                while ((country = sr.ReadLine()) != null)
                {
                    countries.Add(id, country);

                    id++;
                }
            }

            return countries;
        }

        private static Dictionary<int, string> LoadWaterBodies()
        {
            var waterBodies = new Dictionary<int, string>();
            var waterBodiesPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "WaterBodies.txt");

            using (var sr = new StreamReader(waterBodiesPath))
            {
                string waterBody;
                var id = 1;

                while ((waterBody = sr.ReadLine()) != null)
                {
                    waterBodies.Add(id, waterBody);

                    id++;
                }
            }

            return waterBodies;
        }
    }
}