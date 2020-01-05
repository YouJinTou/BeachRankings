using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Models;
using BR.Core.Tools;
using Newtonsoft.Json;
using System.Collections.Generic;
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

        public IEnumerable<IAggregate> GetBeachReviews(EventStream stream)
        {
            var deletedReviewIds = stream
                .Where(e => e.Type.Equals(Event.BeachReviewDeleted))
                .Select(e => e.ToInstance<Review>().Id)
                .ToList();
            var reviews = stream.ToInstances<Review>()
                .Where(r => !deletedReviewIds.Contains(r.Id))
                .GroupBy(r => r.Id)
                .Select(g => g.Last())
                .ToList();

            return reviews;
        }
    }
}
