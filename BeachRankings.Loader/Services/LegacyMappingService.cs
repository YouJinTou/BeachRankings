using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace BeachRankings.Loader.Services
{
    public class LegacyMappingService
    {
        private static string currentContinent;
        private static string currentCountry;
        private static string currentL1;
        private static string currentL2;
        private static string currentL3;
        private static string currentL4;
        private static string currentCountryWaterBody;
        private static string currentL1WaterBody;
        private static string currentL2WaterBody;
        private static string currentL3WaterBody;
        static List<Continent> cts = new List<Continent>();
        static List<Country> cs = new List<Country>();
        static List<L1> pds = new List<L1>();
        static List<L2> sds = new List<L2>();
        static List<L3> tds = new List<L3>();
        static List<L4> qds = new List<L4>();

        public static void Run()
        {
            var seed = GetSeedString();
            var seedJson = Deserialize(seed);

            foreach (var country in (Dictionary<string, object>)seedJson)
            {
                currentCountry = GetPlaceName(country.Key);
                currentContinent = GetContinentName(country.Key);
                currentCountryWaterBody = GetWaterBody(country.Key);
                var countryEntity = new Country()
                {
                    Name = currentCountry,
                    Continent = currentContinent,
                    WaterBody = currentCountryWaterBody
                };

                cs.Add(countryEntity);

                TraverseDivisions(country, 1);
            }

            cts = cs.Select(c => c.Continent).Distinct().Select(c => new Continent
            {
                Name = c
            }).ToList();

            CreateXml();
        }

        private static void CreateXml()
        {
            var doc = new XDocument();
            var seed = new XElement("Seed");
            var continentsCount = 0;
            var countriesCount = 0;
            var pdsCount = 0;
            var sdsCount = 0;
            var tdsCount = 0;
            var qdsCount = 0;
            var sdsDuplicates = new Dictionary<string, int>();

            foreach (var continent in cts)
            {
                continentsCount++;
                var xmlContinent = new XElement("Continent", new XAttribute("name", continent.Name));

                foreach (var country in cs.Where(c => c.Continent == continent.Name))
                {
                    countriesCount++;
                    var xmlCountry = new XElement("Country",
                        new XAttribute("name", country.Name),
                        new XAttribute("continent", country.Continent),
                        new XAttribute("waterBody", country.WaterBody ?? string.Empty));

                    foreach (var pd in pds.Where(p => p.Country == country.Name))
                    {
                        pdsCount++;
                        var xmlPd = new XElement("L1",
                            new XAttribute("name", pd.Name),
                            new XAttribute("continent", pd.Continent),
                            new XAttribute("country", pd.Country),
                            new XAttribute("waterBody", pd.WaterBody ?? string.Empty));

                        foreach (var sd in sds.Where(s => s.L1 == pd.Name))
                        {
                            if (sdsDuplicates.ContainsKey(sd.Name))
                            {
                                sdsDuplicates[sd.Name]++;
                            }
                            else
                            {
                                sdsDuplicates.Add(sd.Name, 1);
                            }
                            sdsCount++;
                            var xmlSd = new XElement("L2",
                                new XAttribute("name", sd.Name),
                                new XAttribute("continent", sd.Continent),
                                new XAttribute("country", sd.Country),
                                new XAttribute("L1", sd.L1),
                                new XAttribute("waterBody", sd.WaterBody ?? string.Empty));

                            foreach (var td in tds.Where(t => t.L2 == sd.Name))
                            {
                                tdsCount++;
                                var xmlTd = new XElement("L3",
                                    new XAttribute("name", td.Name),
                                    new XAttribute("continent", td.Continent),
                                    new XAttribute("country", td.Country),
                                    new XAttribute("L1", td.L1),
                                    new XAttribute("L2", td.L2),
                                    new XAttribute("waterBody", td.WaterBody ?? string.Empty));

                                foreach (var qd in qds.Where(q => q.L3 == td.Name))
                                {
                                    qdsCount++;
                                    var xmlQd = new XElement("L4",
                                        new XAttribute("name", qd.Name),
                                        new XAttribute("continent", qd.Continent),
                                        new XAttribute("country", qd.Country),
                                        new XAttribute("L1", qd.L1),
                                        new XAttribute("L2", qd.L2),
                                        new XAttribute("L3", qd.L3),
                                        new XAttribute("waterBody", qd.WaterBody ?? string.Empty));

                                    xmlTd.Add(xmlQd);
                                }

                                xmlSd.Add(xmlTd);
                            }

                            xmlPd.Add(xmlSd);
                        }

                        xmlCountry.Add(xmlPd);
                    }

                    xmlContinent.Add(xmlCountry);
                }

                seed.Add(xmlContinent);
            }

            doc.Add(seed);
            var xasdasdasdadasda = sdsDuplicates.OrderByDescending(s => s.Value).ToList();
            doc.Save(@"C:\Users\dandr\Desktop\seed.xml");
        }

        private static void TraverseDivisions(object parent, int depth)
        {
            if (parent is string)
            {
                return;
            }

            foreach (var child in GetObjectCollection(parent))
            {
                var savedSuccessfully = SaveDivision(child, depth);
                depth = savedSuccessfully ? depth : depth - 1;

                TraverseDivisions(child, depth + 1);

                depth = savedSuccessfully ? depth : depth + 1;
            }
        }

        private static bool SaveDivision(object division, int depth)
        {
            if (!(division is KeyValuePair<string, object> || division is string))
            {
                return false;
            }

            var value = string.Empty;

            if (division is KeyValuePair<string, object>)
            {
                var kvpDivision = (KeyValuePair<string, object>)division;
                value = kvpDivision.Key;
            }
            else
            {
                value = division.ToString();
            }

            switch (depth)
            {
                case 1:
                    var L1 = new L1()
                    {
                        Name = GetPlaceName(value),
                        Continent = currentContinent,
                        Country = currentCountry,
                        WaterBody = GetWaterBody(value) ?? currentCountryWaterBody
                    };
                    currentL1WaterBody = L1.WaterBody;

                    pds.Add(L1);

                    currentL1 = L1.Name;

                    break;
                case 2:
                    var L2 = new L2()
                    {
                        Name = GetPlaceName(value),
                        Continent = currentContinent,
                        Country = currentCountry,
                        L1 = currentL1,
                        WaterBody = GetWaterBody(value) ?? currentL1WaterBody
                    };
                    currentL2WaterBody = L2.WaterBody;

                    sds.Add(L2);

                    currentL2 = L2.Name;

                    break;
                case 3:
                    var L3 = new L3()
                    {
                        Name = value,
                        Continent = currentContinent,
                        Country = currentCountry,
                        L1 = currentL1,
                        L2 = currentL2,
                        WaterBody = GetWaterBody(value) ?? currentL2WaterBody
                    };
                    currentL3WaterBody = L3.WaterBody;

                    tds.Add(L3);

                    currentL3 = L3.Name;

                    break;
                case 4:
                    var L4 = new L4()
                    {

                        Name = value,
                        Continent = currentContinent,
                        Country = currentCountry,
                        L1 = currentL1,
                        L2 = currentL2,
                        L3 = currentL3,
                        WaterBody = GetWaterBody(value) ?? currentL3WaterBody
                    };

                    qds.Add(L4);

                    currentL4 = L4.Name;

                    break;
                default:
                    throw new ArgumentException("Invalid division.");
            }

            return true;
        }

        private static dynamic GetObjectCollection(object division)
        {
            if (division is Dictionary<string, object>)
            {
                return (Dictionary<string, object>)division;
            }
            else if (division is KeyValuePair<string, object>)
            {
                var kvpDivision = (KeyValuePair<string, object>)division;

                return kvpDivision.Value ?? new List<object>();
            }
            else if (division is List<object>)
            {
                return (List<object>)division;
            }

            throw new ArgumentException("Received an unexpected type.");
        }

        private static string GetSeedString()
        {
            using (var sr = new StreamReader(@"C:\Users\dandr\Desktop\Seed.txt"))
            {
                return sr.ReadToEnd();
            }
        }

        public static object Deserialize(string json)
        {
            return ToObject(JToken.Parse(json));
        }

        private static object ToObject(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return token.Children<JProperty>()
                                .ToDictionary(prop => prop.Name,
                                              prop => ToObject(prop.Value));
                case JTokenType.Array:
                    return token.Select(ToObject).ToList();
                default:
                    return ((JValue)token).Value;
            }
        }

        private static string GetPlaceName(string place)
        {
            var tokens = place.Split(new char[] { '!' }, StringSplitOptions.RemoveEmptyEntries);
            var noWaterBody = (tokens.Length == 1);

            if (noWaterBody)
            {
                var continentIndex = place.IndexOf('%');

                if (continentIndex > -1)
                {
                    place = place.Substring(continentIndex + 1);
                }

                return place;
            }

            var placeName = tokens[1];

            return placeName;
        }

        private static string GetContinentName(string place)
        {
            var tokens = place.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);

            return tokens[0];
        }

        private static string GetWaterBody(string place)
        {
            var continentIndex = place.IndexOf('%');
            place = place.Substring(continentIndex + 1);
            var tokens = place.Split(new char[] { '!' }, StringSplitOptions.RemoveEmptyEntries);
            var noWaterBody = (tokens.Length == 1);

            if (noWaterBody)
            {
                return null;
            }

            var waterBodyName = tokens[0];

            return waterBodyName;
        }
    }

    public class Continent
    {
        public string Name { get; set; }
    }

    public class Country
    {
        public string Name { get; set; }

        public string Continent { get; set; }

        public string WaterBody { get; set; }
    }

    public class L1
    {
        public string Name { get; set; }

        public string Continent { get; set; }

        public string Country { get; set; }

        public string WaterBody { get; set; }
    }

    public class L2
    {
        public string Name { get; set; }

        public string Continent { get; set; }

        public string Country { get; set; }

        public string L1 { get; set; }

        public string WaterBody { get; set; }
    }

    public class L3
    {
        public string Name { get; set; }

        public string Continent { get; set; }

        public string Country { get; set; }

        public string L1 { get; set; }

        public string L2 { get; set; }

        public string WaterBody { get; set; }
    }

    public class L4
    {
        public string Name { get; set; }

        public string Continent { get; set; }

        public string Country { get; set; }

        public string L1 { get; set; }

        public string L2 { get; set; }

        public string L3 { get; set; }

        public string WaterBody { get; set; }
    }

    public class WaterBody
    {
        public string Name { get; set; }
    }
}
