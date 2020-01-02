using BR.Core.Events;
using BR.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface IEventStore
    {
        Task<EventStream> GetEventStreamAsync(string streamId, int offset = 0);

        Task<EventStream> GetEventStreamAsync(string streamId, string type);

        Task<IEnumerable<EventStream>> GetEventStreamsAsync(IEnumerable<string> ids);

        Task AppendEventAsync(EventBase @event);

        Task AppendEventStreamAsync(EventStream stream);

        Task AppendFromStringAsync(string eventString);
    }
}
