﻿using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Tools;
using Microsoft.Extensions.Logging;
using System;
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

                return new EventStream(events.ToList(), offset);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to get events for stream {streamId}/{offset}.");

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

                await this.repo.AddManyAsync(stream);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to append events.");

                throw;
            }
        }
    }
}
