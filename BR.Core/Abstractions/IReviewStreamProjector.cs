using BR.Core.Events;
using System.Collections.Generic;

namespace BR.Core.Abstractions
{
    public interface IReviewStreamProjector : IStreamProjector
    {
        IEnumerable<IAggregate> GetBeachReviews(EventStream stream);
    }
}
