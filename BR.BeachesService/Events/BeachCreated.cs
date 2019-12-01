using BR.BeachesService.Models;
using BR.Core.Events;

namespace BR.BeachesService.Events
{
    public class BeachCreated : EventBase
    {
        public BeachCreated(Beach beach)
            : base(beach.Id, 0, beach)
        {
        }
    }
}
