using BR.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Sourcing
{
    internal class EventStore : IEventStore
    {
        public Task AddEventsAsync(IEnumerable<EventBase> events)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EventBase>> GetEventsAsync(Guid streamId, int offset = 0)
        {
            throw new NotImplementedException();
        }
    }
}
