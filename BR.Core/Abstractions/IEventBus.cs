using BR.Core.Events;
using BR.Core.Models;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface IEventBus
    {
        Task PublishEventAsync(EventBase @event);

        Task PublishEventStreamAsync(EventStream stream);
    }
}
