using BR.Core.Events;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface IEventStore
    {
        Task<EventStream> GetEventStream(string streamId, int offset = 0); 

        Task AppendEventStream(EventStream stream);
    }
}
