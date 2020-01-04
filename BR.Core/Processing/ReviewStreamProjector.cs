using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Models;
using BR.Core.Tools;
using Newtonsoft.Json;
using System.Linq;

namespace BR.Core.Processing
{
    public class ReviewStreamProjector : IReviewStreamProjector
    {
        public IAggregate GetSnapshot(EventStream stream)
        {
            Validator.ThrowIfNull(stream);

            var @event = stream.LastOrDefault();

            return (@event == null) ?
                Review.CreateNull() : JsonConvert.DeserializeObject<Review>(@event.Body);
        }
    }
}
