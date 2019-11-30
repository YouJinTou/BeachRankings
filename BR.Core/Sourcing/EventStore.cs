using BR.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Core.Sourcing
{
    internal class EventStore : IEventStore
    {
        private readonly INoSqlRepository<EventBase> repo;
        private readonly ILogger<EventStore> logger;

        public EventStore(INoSqlRepository<EventBase> repo, ILogger<EventStore> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        public async Task AddEventsAsync(IEnumerable<EventBase> events)
        {
            try
            {
                await this.repo.AddManyAsync(events);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to get add events.");

                throw;
            }
        }

        public async Task<IEnumerable<EventBase>> GetEventsAsync(string streamId, int offset = 0)
        {
            try
            {
                var events = await this.repo.GetManyAsync(streamId);
                
                if (offset > 0)
                {
                    events = events.Where(e => e.Offset >= offset);
                }

                return events.ToList();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to get events for stream {streamId}/{offset}.");

                throw;
            }
        }
    }
}
