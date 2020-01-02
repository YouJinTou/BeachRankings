using BR.BeachesService.Models;
using BR.Core.Models;

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
