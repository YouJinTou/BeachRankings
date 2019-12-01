using BR.Core.Events;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface IEventStore
    {
        Task<EventStream> GetEventStreamAsync(string streamId, int offset = 0);

        Task<EventStream> GetEventStreamByTypeAsync(string streamId, string type);

        Task AppendEventAsync(EventBase @event);

        Task AppendEventStreamAsync(EventStream stream);
    }
}
