using BR.Core.Events;

namespace BR.Core.Abstractions
{
    internal interface IEventsRepository : INoSqlRepository<EventBase>
    {
    }
}
