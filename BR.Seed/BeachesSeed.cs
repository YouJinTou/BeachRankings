using BR.BeachesService.Models;
using BR.Core.Cloud.Aws;
using BR.Core.Models;
using BR.Core.Tools;
using BR.IndexService.Processors;
using BR.Seed.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Seed
{
    public static class BeachesSeed
    {
        private static IndexEntryPreprocessor preprocessor = new IndexEntryPreprocessor();

        public static async Task SeedBeachesAsync()
        {
            var db = new DynamoRepository<IndexEntry>("Index");
            using var reader = new StreamReader("beaches.csv");
            var allIndices = new List<IndexEntry>();
            var line = string.Empty;

            while ((line = reader.ReadLine()) != null)
            {
                var tokens = line.Split('\t');
                var indices = GetBeachIndices(tokens).ToList();

                indices.ForEach(i => allIndices.Add(i));
            }

            var groupedIndices = allIndices.Group();

            await db.AddManyAsync(groupedIndices);
        }

        private static IEnumerable<IndexEntry> GetBeachIndices(string[] tokens)
        {
            var name = tokens[0];
            var continent = tokens[1];
            var country = tokens[2];
            var l1 = tokens[3];
            var l2 = tokens[4];
            var l3 = tokens[5];
            var l4 = tokens[6];
            var waterBody = tokens[7];
            var beach = new Beach(
                name: name,
                continent: continent,
                waterBody: waterBody,
                country: country,
                l1: l1,
                l2: l2,
                l3: l3,
                l4: l4);
            var id = Beach.GetId(beach);
            var allEntries = Collection.Combine<IndexEntry>(
                GetPlaceEntries(PlaceType.Beach, name, id),
                GetPlaceEntries(PlaceType.Continent, continent, id),
                GetPlaceEntries(PlaceType.Country, country, id),
                GetPlaceEntries(PlaceType.L1, l1, id),
                GetPlaceEntries(PlaceType.L2, l2, id),
                GetPlaceEntries(PlaceType.L3, l3, id),
                GetPlaceEntries(PlaceType.L4, l4, id),
                GetPlaceEntries(PlaceType.WaterBody, waterBody, id));

            return allEntries;
        }

        private static IEnumerable<IndexEntry> GetPlaceEntries(
            PlaceType type, string name, params string[] ids)
        {
            if (name.ToLower() == "null")
            {
                return new List<IndexEntry>();
            }

            return preprocessor.PreprocessToken(new IndexToken(name, type), ids);
        }
    }
}
