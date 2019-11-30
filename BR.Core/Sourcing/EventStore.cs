using BR.Core.Abstractions;
using BR.Core.Events;
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

        public async Task AppendEventStream(EventStream stream)
        {
            try
            {
                await this.repo.AddManyAsync(stream);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to get add events.");

                throw;
            }
        }

        public async Task<EventStream> GetEventStream(string streamId, int offset = 0)
        {
            try
            {
                var events = await this.repo.GetManyAsync(streamId);

                return new EventStream(events.ToList(), offset);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to get events for stream {streamId}/{offset}.");

                throw;
            }
        }
    }
}
