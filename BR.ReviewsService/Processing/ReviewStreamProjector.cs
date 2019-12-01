using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Tools;
using BR.ReviewsService.Models;
using Newtonsoft.Json;
using System.Linq;

namespace BR.ReviewsService.Processing
{
    public class ReviewStreamProjector : IStreamProjector
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
