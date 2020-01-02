using BR.Core.Models;

namespace BR.Core.Abstractions
{
    internal interface IEventsRepository : INoSqlRepository<EventBase>
    {
    }
}
