using BR.Core.Events;
using BR.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface IEventStore
    {
        Task<EventStream> GetEventStreamAsync(string streamId);

        Task<EventStream> GetEventStreamAsync(string streamId, string type);

        Task<IEnumerable<EventStream>> GetEventStreamsAsync(IEnumerable<string> ids);

        Task AppendEventAsync(AppEvent @event);

        Task AppendEventStreamAsync(EventStream stream);

        Task AppendFromStringAsync(string eventString);
    }
}
