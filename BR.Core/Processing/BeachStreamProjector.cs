using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Models;
using BR.Core.Tools;
using Newtonsoft.Json;
using System.Linq;

namespace BR.Core.Processing
{
    public class BeachStreamProjector : IBeachStreamProjector
    {
        public IAggregate GetSnapshot(EventStream stream)
        {
            Validator.ThrowIfNull(stream);

            var @event = stream.Where(
                e => e.Type.Equals(Event.BeachCreated.ToString()) 
                || e.Type.Equals(Event.BeachModified.ToString())
                || e.Type.Equals(Event.BeachDeleted.ToString())).LastOrDefault();

            if (@event == null || @event.Type.Equals(Event.BeachDeleted.ToString()))
            {
                return Beach.CreateNull();
            }

            var beach = JsonConvert.DeserializeObject<Beach>(@event.Body);

            return beach;
        }
    }
}
