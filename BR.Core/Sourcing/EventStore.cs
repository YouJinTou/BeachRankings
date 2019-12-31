using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Extensions;
using BR.Core.Models;
using BR.Core.Tools;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Core.Sourcing
{
    internal class EventStore : IEventStore
    {
        private readonly IEventsRepository repo;
        private readonly ILogger<EventStore> logger;

        public EventStore(IEventsRepository repo, ILogger<EventStore> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        public async Task<EventStream> GetEventStreamAsync(string streamId, int offset = 0)
        {
            try
            {
                Validator.ThrowIfNullOrWhiteSpace(streamId);

                this.logger.LogInformation($"Getting event stream for {streamId}/{offset}.");

                var events = await this.repo.GetManyAsync(streamId);

                return new EventStream(events, offset);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to get events for stream {streamId}/{offset}.");

                throw;
            }
        }

        public async Task<EventStream> GetEventStreamAsync(string streamId, string type)
        {
            try
            {
                Validator.ThrowIfAnyNullOrWhiteSpace(streamId, type);

                this.logger.LogInformation($"Getting event stream for {streamId}/{type}.");

                var events = await this.repo.GetManyByAttributeAsync(
                    streamId, nameof(EventBase.Type), type);

                return new EventStream(events.ToList());
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to get events for stream {streamId}/{type}.");

                throw;
            }
        }

        public async Task AppendEventAsync(EventBase @event)
        {
            try
            {
                Validator.ThrowIfNull(@event);

                this.logger.LogInformation($"Appending event {@event}.");

                await this.repo.AddAsync(@event);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    ex, $"Failed to append event {@event?.ToString() ?? string.Empty}.");

                throw;
            }
        }

        public async Task AppendEventStreamAsync(EventStream stream)
        {
            try
            {
                Validator.ThrowIfNull(stream);

                this.logger.LogInformation($"Appending event stream {stream}.");

                await this.repo.AddManyAsync(stream);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to append events.");

                throw;
            }
        }

        public async Task AppendFromStringAsync(string eventString)
        {
            try
            {
                Validator.ThrowIfNullOrWhiteSpace(eventString);

                var events = eventString.StartsWith("[") ?
                    JsonConvert.DeserializeObject<IEnumerable<EventBaseModel>>(eventString) :
                    new List<EventBaseModel> 
                    { 
                        JsonConvert.DeserializeObject<EventBaseModel>(eventString) 
                    };
                var stream = EventStream.CreateStream(events.ToArray());

                this.logger.LogInformation($"Appending event stream {stream}.");

                await this.repo.AddManyAsync(stream);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to append event string {eventString}.");

                throw;
            }
        }

        public async Task<IEnumerable<EventStream>> GetEventStreams(params string[] types)
        {
            try
            {
                if (types.IsNullOrEmpty())
                {
                    return new List<EventStream>();
                }

                return null;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to fetch event streams.");

                throw;
            }
        }
    }
}
