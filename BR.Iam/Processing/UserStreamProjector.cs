using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Tools;
using BR.Iam.Models;
using Newtonsoft.Json;
using System.Linq;

namespace BR.Iam.Processing
{
    public class UserStreamProjector : IStreamProjector
    {
        public IAggregate GetSnapshot(EventStream stream)
        {
            Validator.ThrowIfNull(stream);

            var @event = stream.LastOrDefault();

            return (@event == null) ?
                User.CreateNull() : JsonConvert.DeserializeObject<User>(@event.Body);
        }
    }
}
