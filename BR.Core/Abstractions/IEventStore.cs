using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface IEventStore
    {
        Task<IEnumerable<EventBase>> GetEventsAsync(Guid streamId, int offset = 0); 

        Task AddEventsAsync(IEnumerable<EventBase> events);
    }
}
