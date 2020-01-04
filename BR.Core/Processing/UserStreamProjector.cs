using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Models;
using BR.Core.Tools;
using Newtonsoft.Json;
using System.Linq;

namespace BR.Core.Processing
{
    public class UserStreamProjector : IUserStreamProjector
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
