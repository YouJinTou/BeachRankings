using BR.Core.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface IEventStore
    {
        Task<EventStream> GetEventStreamAsync(string streamId, int offset = 0);

        Task<EventStream> GetEventStreamAsync(string streamId, string type);

        Task AppendEventAsync(EventBase @event);

        Task AppendEventStreamAsync(EventStream stream);

        Task AppendFromStringAsync(string eventString);

        Task<IEnumerable<EventStream>> GetEventStreams(params string[] types);
    }
}
