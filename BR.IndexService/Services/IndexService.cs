using BR.Core.Abstractions;
using BR.Core.Extensions;
using BR.Core.Models;
using BR.Core.Tools;
using BR.IndexService.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.IndexService.Services
{
    public class IndexService : IIndexService
    {
        private class PlaceKey
        {
            public PlaceKey(PlaceType type, IndexKey key)
            {
                this.PlaceType = type;
                this.Key = key;
            }

            public PlaceType PlaceType { get; set; }

            public IndexKey Key { get; set; }
        }

        private readonly INoSqlRepository<IndexEntry> repo;
        private readonly IIndexEntryPreprocessor preprocessor;
        private readonly ILogger<IndexService> logger;

        public IndexService(
            INoSqlRepository<IndexEntry> repo, 
            IIndexEntryPreprocessor preprocessor, 
            ILogger<IndexService> logger)
        {
            this.repo = repo;
            this.preprocessor = preprocessor;
            this.logger = logger;
        }

        public async Task UpdateIndexAsync(string eventString) 
        {
            try
            {
                Validator.ThrowIfNullOrWhiteSpace(eventString, $"Missing event string.");

                var @event = JsonConvert.DeserializeObject<AppEvent>(eventString);
                var beach = JsonConvert.DeserializeObject<IndexBeach>(@event.Body);

                if (!@event.Type.StartsWith("Beach") || @event.Type.Equals("BeachDeleted"))
                {
                    this.logger.LogInformation($"Skipping {@event.Type}.");

                    return;
                }

                var placeKeys = this.GetPlaceKeys(beach);
                var allEntries = new List<IndexEntry>();

                foreach (var placeKey in placeKeys)
                {
                    var entries = await this.repo.GetManyBeginsWithAsync(
                        "Bucket", placeKey.Key.Bucket, "Token", placeKey.Key.Token);

                    if (entries.IsNullOrEmpty())
                    {
                        entries = this.preprocessor.PreprocessToken(
                            new IndexToken(placeKey.Key.Token, placeKey.PlaceType), beach);
                    }
                    else
                    {
                        foreach (var entry in entries)
                        {
                            foreach (var posting in entry.Postings)
                            {
                                var indexBeach = posting.Beaches.FirstOrDefault(b => b.Id == beach.Id);

                                if (indexBeach == null)
                                {
                                    posting.Beaches = new List<IndexBeach>(posting.Beaches)
                                    {
                                        beach
                                    };
                                }
                                else
                                {
                                    indexBeach = beach;
                                }
                            }
                        }
                    }

                    allEntries.AddRange(entries);
                }

                var groupedEntries = allEntries.Group();

                await this.repo.AddManyAsync(groupedEntries);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to update index.");

                throw;
            }
        }

        private IEnumerable<PlaceKey> GetPlaceKeys(IndexBeach beach)
        {
            var keys = new List<PlaceKey>
            {
                new PlaceKey(PlaceType.Beach, new IndexKey(beach.Name)),
                new PlaceKey(PlaceType.Continent, new IndexKey(beach.Continent)),
                new PlaceKey(PlaceType.Country, new IndexKey(beach.Country)),
                new PlaceKey(PlaceType.L1, new IndexKey(beach.L1)),
                new PlaceKey(PlaceType.L2, new IndexKey(beach.L2)),
                new PlaceKey(PlaceType.L3, new IndexKey(beach.L3)),
                new PlaceKey(PlaceType.L4, new IndexKey(beach.L4)),
                new PlaceKey(PlaceType.WaterBody, new IndexKey(beach.WaterBody))
            }
            .Where(pk => !string.IsNullOrWhiteSpace(pk.Key.Bucket))
            .ToList();

            return keys;
        }
    }
}
