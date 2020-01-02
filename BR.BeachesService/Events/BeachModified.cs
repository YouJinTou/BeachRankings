using BR.BeachesService.Models;
using BR.Core.Models;

namespace BR.BeachesService.Events
{
    public class BeachModified : EventBase
    {
        public BeachModified(string id, int offset, ModifyBeachModel model)
            : base(id, offset, model)
        {
        }
    }
}
