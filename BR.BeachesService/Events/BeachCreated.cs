using BR.BeachesService.Models;
using BR.Core.Events;
using Newtonsoft.Json;

namespace BR.BeachesService.Events
{
    public class BeachCreated : EventBase
    {
        public BeachCreated(Beach beach)
            : base(beach.Id, 0, JsonConvert.SerializeObject(beach))
        {
        }
    }
}
