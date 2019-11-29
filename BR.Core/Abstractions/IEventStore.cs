using BR.Core.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface IEventStore
    {
        Task AddEventsAsync(IEnumerable<EventBase> events);
    }
}
