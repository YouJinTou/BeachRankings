using BR.Core.Events;

namespace BR.Core.Abstractions
{
    public interface IStreamProjector
    {
        IAggregate GetSnapshot(EventStream stream);
    }
}
