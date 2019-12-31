using BR.BeachesService.Models;
using BR.Core.Cloud.Aws;
using BR.Core.Models;
using BR.Core.Tools;
using BR.IndexService.Processors;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Seed
{
    public static class BeachesSeed
    {
        public static async Task SeedBeachesAsync()
        {
            var db = new DynamoRepository<IndexEntry>("Index");
            using var reader = new StreamReader("beaches.csv");
            var entryPreprocessor = new IndexEntryPreprocessor();
            var allIndices = new HashSet<IndexEntry>();
            var line = string.Empty;

            while ((line = reader.ReadLine()) != null)
            {
                var tokens = line.Split('\t');
                var indices = GetBeachIndices(tokens, entryPreprocessor).ToList();

                indices.ForEach(i => allIndices.Add(i));
            }

            var groupedIndices = GroupIndices(allIndices);

            await db.AddManyAsync(groupedIndices);
        }

        private static IEnumerable<IndexEntry> GetBeachIndices(
            string[] tokens, IndexEntryPreprocessor preprocessor)
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
            var beachEntries = preprocessor.PreprocessToken(
                new IndexToken(name, PlaceType.Beach), id);
            var continentEntries = preprocessor.PreprocessToken(
                new IndexToken(continent, PlaceType.Continent), id);
            var countryEntries = preprocessor.PreprocessToken(
               new IndexToken(country, PlaceType.Country), id);
            var l1Entries = preprocessor.PreprocessToken(
               new IndexToken(l1, PlaceType.L1), id);
            var l2Entries = preprocessor.PreprocessToken(
               new IndexToken(l2, PlaceType.L2), id);
            var l3Entries = preprocessor.PreprocessToken(
               new IndexToken(l3, PlaceType.L3), id);
            var l4Entries = preprocessor.PreprocessToken(
               new IndexToken(l4, PlaceType.L4), id);
            var waterBodyEntries = preprocessor.PreprocessToken(
               new IndexToken(waterBody, PlaceType.WaterBody), id);
            var allEntries = Collection.Combine<IndexEntry>(
                beachEntries, 
                continentEntries,
                countryEntries,
                l1Entries,
                l2Entries,
                l3Entries,
                l4Entries,
                waterBodyEntries);

            return allEntries;
        }

        private static IEnumerable<IndexEntry> GroupIndices(IEnumerable<IndexEntry> entries)
        {
            var groupedEntries = new Dictionary<string, IndexEntry>();

            foreach (var entry in entries)
            {
                if (groupedEntries.ContainsKey(entry.ToString()))
                {
                    var currentEntry = groupedEntries[entry.ToString()];
                    var newPostings = new HashSet<string>(
                        Collection.Combine<string>(currentEntry.Postings, entry.Postings));
                    currentEntry.Postings = newPostings;
                }
                else
                {
                    groupedEntries[entry.ToString()] = entry;
                }
            }

            return groupedEntries.Values;
        }
    }
}
