using BR.BeachesService.Events;
using BR.BeachesService.Models;
using BR.Core.Abstractions;
using BR.Core.Cloud.Aws;
using BR.Core.Events;
using BR.Core.Extensions;
using BR.Core.Models;
using BR.Core.Tools;
using BR.IndexService.Processors;
using BR.Seed.Extensions;
using Microsoft.Extensions.DependencyInjection;
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
            var allBeachTokens = new List<string[]>();
            var line = string.Empty;

            while ((line = reader.ReadLine()) != null)
            {
                var tokens = line.Split('\t');
                var indices = GetBeachIndices(tokens).ToList();

                indices.ForEach(i => allIndices.Add(i));

                allBeachTokens.Add(tokens);
            }

            var groupedIndices = allIndices.Group();

            //await db.AddManyAsync(groupedIndices);

            await SeedEventsAsync(allBeachTokens);
        }

        private static IEnumerable<IndexEntry> GetBeachIndices(string[] tokens)
        {
            var beach = CreateBeach(tokens);
            var allEntries = Collection.Combine<IndexEntry>(
                GetPlaceEntries(PlaceType.Beach, beach.Name, beach.Id),
                GetPlaceEntries(PlaceType.Continent, beach.Continent, beach.Id),
                GetPlaceEntries(PlaceType.Country, beach.Country, beach.Id),
                GetPlaceEntries(PlaceType.L1, beach.L1, beach.Id),
                GetPlaceEntries(PlaceType.L2, beach.L2, beach.Id),
                GetPlaceEntries(PlaceType.L3, beach.L3, beach.Id),
                GetPlaceEntries(PlaceType.L4, beach.L4, beach.Id),
                GetPlaceEntries(PlaceType.WaterBody, beach.WaterBody, beach.Id));

            return allEntries;
        }

        private static IEnumerable<IndexEntry> GetPlaceEntries(
            PlaceType type, string name, params string[] ids)
        {
            if (string.IsNullOrWhiteSpace(name.NullIfNullString()))
            {
                return new List<IndexEntry>();
            }

            return preprocessor.PreprocessToken(new IndexToken(name, type), ids);
        }

        private static async Task SeedEventsAsync(IEnumerable<string[]> tokensList)
        {
            var services = new ServiceCollection().AddCore();
            var provider = services.BuildServiceProvider();
            var store = provider.GetService<IEventStore>();
            var beaches = tokensList.Select(t => CreateBeach(t)).ToList();
            var events = beaches.Select(b => new BeachCreated(b)).ToArray();

            await store.AppendEventStreamAsync(EventStream.CreateStream(events));
        }

        private static Beach CreateBeach(string[] tokens)
        {
            var name = tokens[0];
            var continent = tokens[1];
            var country = tokens[2].NullIfNullString();
            var l1 = tokens[3].NullIfNullString();
            var l2 = tokens[4].NullIfNullString();
            var l3 = tokens[5].NullIfNullString();
            var l4 = tokens[6].NullIfNullString();
            var waterBody = tokens[7];
            var coordinates = tokens[8].NullIfNullString();
            var score = tokens[9];
            var sandQuality = tokens[10];
            var beachCleanliness = tokens[11];
            var beautifulScenery = tokens[12];
            var crowdFree = tokens[13];
            var infrastructure = tokens[14];
            var waterVisibility = tokens[15];
            var litterFree = tokens[16];
            var feetFriendlyBottom = tokens[17];
            var seaLifeDiversity = tokens[18];
            var coralReef = tokens[19];
            var snorkeling = tokens[20];
            var kayaking = tokens[21];
            var walking = tokens[22];
            var camping = tokens[23];
            var longTermStay = tokens[24];
            var beach = new Beach(
                name: name,
                continent: continent,
                waterBody: waterBody,
                country: country,
                l1: l1,
                l2: l2,
                l3: l3,
                l4: l4,
                addedBy: "Admin",
                coordinates: coordinates,
                score: ParseNullDouble(score),
                sandQuality: ParseNullDouble(sandQuality),
                beachCleanliness: ParseNullDouble(beachCleanliness),
                beautifulScenery: ParseNullDouble(beautifulScenery),
                crowdFree: ParseNullDouble(crowdFree),
                infrastructure: ParseNullDouble(infrastructure),
                waterVisibility: ParseNullDouble(waterVisibility),
                litterFree: ParseNullDouble(litterFree),
                feetFriendlyBottom: ParseNullDouble(feetFriendlyBottom),
                seaLifeDiversity: ParseNullDouble(seaLifeDiversity),
                coralReef: ParseNullDouble(coralReef),
                snorkeling: ParseNullDouble(snorkeling),
                kayaking: ParseNullDouble(kayaking),
                walking: ParseNullDouble(walking),
                camping: ParseNullDouble(camping),
                longTermStay: ParseNullDouble(longTermStay));

            return beach;
        }

        private static double? ParseNullDouble(string number)
        {
            return string.IsNullOrEmpty(number.NullIfNullString()) ? 
                (double?)null : 
                double.Parse(number);
        }
    }
}
