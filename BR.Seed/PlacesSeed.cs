using BR.Core.Cloud.Aws;
using BR.Core.Extensions;
using BR.Core.Models;
using BR.Seed.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BR.Seed
{
    public static class PlacesSeed
    {
        public static async Task SeedPlacesAsync()
        {
            var db = new DynamoRepository<Place>("Places");
            var places = GetPlaces();

            await db.AddManyAsync(places);
        }

        private static IEnumerable<Place> GetPlaces()
        {
            using var reader = new StreamReader("seed.xml");
            var serializer = new XmlSerializer(typeof(Seed));
            var seed = (Seed)serializer.Deserialize(reader);
            var places = new List<Place>();

            foreach (var continent in seed.Continent)
            {
                var conWb = new HashSet<string>();

                foreach (var country in continent.Country)
                {
                    var couWb = new HashSet<string>();

                    country.waterBody.AddTo(conWb, couWb);

                    foreach (var l1 in country.L1.NewIfNull())
                    {
                        var l1Wb = new HashSet<string>();

                        l1.waterBody.AddTo(conWb, couWb, l1Wb);

                        foreach (var l2 in l1.L2.NewIfNull())
                        {
                            var l2Wb = new HashSet<string>();

                            l2.waterBody.AddTo(conWb, couWb, l1Wb, l2Wb);

                            foreach (var l3 in l2.L3.NewIfNull())
                            {
                                var l3Wb = new HashSet<string>();

                                l3.waterBody.AddTo(conWb, couWb, l1Wb, l2Wb, l3Wb);

                                foreach (var l4 in l3.L4.NewIfNull())
                                {
                                    var l4Wb = new HashSet<string>();

                                    l4.waterBody.AddTo(conWb, couWb, l1Wb, l2Wb, l3Wb, l4Wb);

                                    places.Add(l4.GetL4(l4Wb));
                                }

                                places.Add(l3.GetL3(l3Wb));
                            }

                            places.Add(l2.GetL2(l2Wb));
                        }

                        places.Add(l1.GetL1(l1Wb));
                    }

                    places.AddRange(country.GetCountry(couWb));
                }

                places.Add(continent.GetContinent(conWb));
            }

            places.AddRange(GetWaterBodyPlaces(places));

            DoSanityCheck(places);

            return places;
        }

        private static IEnumerable<Place> GetWaterBodyPlaces(IEnumerable<Place> places)
        {
            var waterBodies = places
                .Select(p => p.WaterBodies)
                .SelectMany(wb => wb)
                .Distinct()
                .ToList();
            var waterBodyPlaces = new List<Place>();

            foreach (var waterBody in waterBodies)
            {
                var currentPlaces = places.Where(p => p.WaterBodies.Contains(waterBody)).ToList();

                waterBodyPlaces.Add(new Place
                {
                    Id = waterBody,
                    Children = currentPlaces.Select(wbp => wbp.Id),
                    Type = PlaceType.WaterBody.ToString(),
                    Name = waterBody,
                    Continent = GetWaterBodyPlace(currentPlaces, PlaceType.Continent),
                    Country = GetWaterBodyPlace(currentPlaces, PlaceType.Country),
                    L1 = GetWaterBodyPlace(currentPlaces, PlaceType.L1),
                    L2 = GetWaterBodyPlace(currentPlaces, PlaceType.L2),
                    L3 = GetWaterBodyPlace(currentPlaces, PlaceType.L3),
                    L4 = GetWaterBodyPlace(currentPlaces, PlaceType.L4)
                });
            }

            return waterBodyPlaces;
        }

        private static string GetWaterBodyPlace(IEnumerable<Place> places, PlaceType type)
        {
            var results = places
                .Where(p => p.Type == type.ToString()).Select(p => p.Id).Distinct();
            var result = string.Join('*', results);

            return result.NullIfEmpty();
        }

        private static void DoSanityCheck(IEnumerable<Place> places)
        {
            var duplicatesExist = false;

            foreach (var place in places)
            {
                var duplicatesCounter = new Dictionary<string, int>();

                foreach (var child in place.Children.NewIfNull())
                {
                    if (duplicatesCounter.ContainsKey(child))
                    {
                        duplicatesCounter[child] += 1;
                    }
                    else
                    {
                        duplicatesCounter.Add(child, 1);
                    }
                }

                foreach (var item in duplicatesCounter.Where(kvp => kvp.Value > 1))
                {
                    duplicatesExist = true;

                    System.Console.WriteLine(item.Key);
                }
            }

            if (duplicatesExist)
            {
                throw new System.InvalidOperationException("Duplicates exist.");
            }
        }
    }
}
