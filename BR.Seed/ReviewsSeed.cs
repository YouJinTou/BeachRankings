using BR.Core;
using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Extensions;
using BR.Core.Models;
using BR.Core.Tools;
using BR.Seed.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Seed
{
    internal static class ReviewsSeed
    {
        private static readonly IEventStore store;

        static ReviewsSeed()
        {
            var services = new ServiceCollection().AddCore();
            var provider = services.BuildServiceProvider();
            store = provider.GetService<IEventStore>();
        }

        public static async Task SeedReviewsAsync()
        {
            var tokensList = new List<string[]>();
            using var csvParser = new TextFieldParser("reviews.csv")
            {
                TextFieldType = FieldType.Delimited,
                Delimiters = new[] { "," }
            };

            while (!csvParser.EndOfData)
            {
                tokensList.Add(csvParser.ReadFields());
            }

            var beaches = BeachesSeed.ParseBeaches();

            await SeedEventsAsync(tokensList, beaches);
        }

        private static async Task SeedEventsAsync(
            IEnumerable<string[]> tokensList, IEnumerable<Beach> beaches)
        {
            var startingIndex = beaches.Count() + 1;
            var reviews = new List<Review>();
            var failedReviews = new List<string>();

            foreach (var list in tokensList)
            {
                try
                {
                    reviews.Add(CreateReview(list, beaches));
                }
                catch
                {
                    Console.WriteLine($"{list[0]} failed.");
                }
            }

            var reviewCreatedEvents = reviews.Select(r => new AppEvent(
                    r.Id.ToString(), r, Event.ReviewCreated.ToString()))
                .ToArray();
            var userLeftReviewEvents = await reviews.SelectDelayAsync(r =>
            {
                var model = new UserLeftReviewModel(r.UserId, r.Id, r.BeachId);

                return new AppEvent(
                    model.UserId, model, Event.UserLeftReview.ToString());
            });
            var beachReviewedEvents = await reviews.SelectDelayAsync(r =>
            {
                var model = new BeachReviewedModel(r.BeachId, r.UserId, r.Id);

                return new AppEvent(
                    model.BeachId, model, Event.BeachReviewed.ToString());
            });
            var events = Collection.Combine<AppEvent>(
                reviewCreatedEvents, userLeftReviewEvents, beachReviewedEvents).ToArray();
            var stream = EventStream.CreateStream(events);

            await store.AppendEventStreamAsync(stream);
        }

        private static Review CreateReview(string[] tokens, IEnumerable<Beach> beaches)
        {
            var nullString = "NULL";
            var matches = beaches.Where(b => b.Name.Equals(tokens[0])).ToList();

            if (matches.Count != 1)
            {
                throw new InvalidOperationException("Cannot determine beach ID.");
            }

            return new Review
            {
                Id = Guid.NewGuid(),
                BeachId = matches.First().Id,
                UserId = Constants.Surfer,
                AddedOn = DateTime.Parse(tokens[1]),
                LastUpdatedOn = DateTime.UtcNow,
                Text = tokens[2].NullIfNullString(nullString),
                Score = tokens[3].ToNullDouble(nullString),
                SandQuality = tokens[4].ToNullDouble(nullString),
                BeachCleanliness = tokens[5].ToNullDouble(nullString),
                BeautifulScenery = tokens[6].ToNullDouble(nullString),
                CrowdFree = tokens[7].ToNullDouble(nullString),
                Infrastructure = tokens[8].ToNullDouble(nullString),
                WaterVisibility = tokens[9].ToNullDouble(nullString),
                LitterFree = tokens[10].ToNullDouble(nullString),
                FeetFriendlyBottom = tokens[11].ToNullDouble(nullString),
                SeaLifeDiversity = tokens[12].ToNullDouble(nullString),
                CoralReef = tokens[13].ToNullDouble(nullString),
                Snorkeling = tokens[14].ToNullDouble(nullString),
                Kayaking = tokens[15].ToNullDouble(nullString),
                Walking = tokens[16].ToNullDouble(nullString),
                Camping = tokens[17].ToNullDouble(nullString),
                LongTermStay = tokens[18].ToNullDouble(nullString)
            };
        }
    }
}
