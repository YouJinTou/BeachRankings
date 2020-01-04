using BR.BeachesService.Models;
using BR.Core;
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
        private const string NullString = "NULL";

        private static IndexEntryPreprocessor preprocessor = new IndexEntryPreprocessor();

        public static IEnumerable<Beach> ParseBeaches()
        {
            using var reader = new StreamReader("beaches.csv");
            var beaches = new List<Beach>();
            var line = string.Empty;

            while ((line = reader.ReadLine()) != null)
            {
                var tokens = line.Split('\t');

                beaches.Add(CreateBeach(tokens));
            }

            return beaches;
        }

        public static async Task SeedBeachesAsync()
        {
            var db = new DynamoRepository<IndexEntry>("Index");
            var beaches = ParseBeaches();
            var indices = beaches.Select(b => GetBeachIndices(b)).SelectMany(i => i).ToList();
            var groupedIndices = indices.Group();

            await db.AddManyAsync(groupedIndices);

            await SeedEventsAsync(beaches);
        }

        private static IEnumerable<IndexEntry> GetBeachIndices(Beach beach)
        {
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
            if (string.IsNullOrWhiteSpace(name.NullIfNullString(NullString)))
            {
                return new List<IndexEntry>();
            }

            return preprocessor.PreprocessToken(new IndexToken(name, type), ids);
        }

        private static async Task SeedEventsAsync(IEnumerable<Beach> beaches)
        {
            var services = new ServiceCollection().AddCore();
            var provider = services.BuildServiceProvider();
            var store = provider.GetService<IEventStore>();
            var events = beaches.Select(
                b => new EventBase(b.Id, b, Event.BeachCreated.ToString())).ToArray();

            await store.AppendEventStreamAsync(EventStream.CreateStream(events));
        }

        private static Beach CreateBeach(string[] tokens)
        {
            var name = tokens[0];
            var continent = tokens[1];
            var country = tokens[2].NullIfNullString(NullString);
            var l1 = tokens[3].NullIfNullString(NullString);
            var l2 = tokens[4].NullIfNullString(NullString);
            var l3 = tokens[5].NullIfNullString(NullString);
            var l4 = tokens[6].NullIfNullString(NullString);
            var waterBody = tokens[7];
            var coordinates = tokens[8].NullIfNullString(NullString);
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
                addedBy: Constants.Surfer,
                coordinates: coordinates,
                score: score.ToNullDouble(NullString),
                sandQuality: sandQuality.ToNullDouble(NullString),
                beachCleanliness: beachCleanliness.ToNullDouble(NullString),
                beautifulScenery: beautifulScenery.ToNullDouble(NullString),
                crowdFree: crowdFree.ToNullDouble(NullString),
                infrastructure: infrastructure.ToNullDouble(NullString),
                waterVisibility: waterVisibility.ToNullDouble(NullString),
                litterFree: litterFree.ToNullDouble(NullString),
                feetFriendlyBottom: feetFriendlyBottom.ToNullDouble(NullString),
                seaLifeDiversity: seaLifeDiversity.ToNullDouble(NullString),
                coralReef: coralReef.ToNullDouble(NullString),
                snorkeling: snorkeling.ToNullDouble(NullString),
                kayaking: kayaking.ToNullDouble(NullString),
                walking: walking.ToNullDouble(NullString),
                camping: camping.ToNullDouble(NullString),
                longTermStay: longTermStay.ToNullDouble(NullString));

            return beach;
        }
    }
}
