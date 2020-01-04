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

        public async Task<EventStream> GetEventStreamAsync(string streamId)
        {
            try
            {
                Validator.ThrowIfNullOrWhiteSpace(streamId);

                this.logger.LogInformation($"Getting event stream for {streamId}.");

                var events = await this.repo.GetManyAsync(Constants.StreamId, streamId);

                return new EventStream(events);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to get events for stream {streamId}.");

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
                    streamId, nameof(AppEvent.Type), type);

                return new EventStream(events.ToList());
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to get events for stream {streamId}/{type}.");

                throw;
            }
        }

        public async Task<IEnumerable<EventStream>> GetEventStreamsAsync(IEnumerable<string> ids)
        {
            try
            {
                Validator.ThrowIfNull(ids);

                this.logger.LogInformation($"Getting event streams for {string.Join(" ", ids)}.");

                var streams = new List<EventStream>();

                foreach (var id in ids)
                {
                    var stream = new EventStream(
                        await this.repo.GetManyAsync(Constants.StreamId, id));

                    streams.Add(stream);
                }

                return streams;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to get streams.");

                throw;
            }
        }

        public async Task AppendEventAsync(AppEvent @event)
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
                    JsonConvert.DeserializeObject<IEnumerable<AppEvent>>(eventString) :
                    new List<AppEvent> 
                    { 
                        JsonConvert.DeserializeObject<AppEvent>(eventString) 
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
    }
}
