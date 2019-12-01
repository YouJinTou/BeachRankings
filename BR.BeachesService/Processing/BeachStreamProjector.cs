using BR.BeachesService.Models;
using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Tools;
using Newtonsoft.Json;
using System.Linq;

namespace BR.BeachesService.Processing
{
    public class BeachStreamProjector : IStreamProjector
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
