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

            var @event = stream.LastOrDefault();

            return (@event == null) ?
                Beach.CreateNull() : JsonConvert.DeserializeObject<Beach>(@event.Body);
        }
    }
}
