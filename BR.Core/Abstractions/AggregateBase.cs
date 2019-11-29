using System;

namespace BR.Core.Abstractions
{
    public abstract class AggregateBase
    {
        public abstract Guid Id { get; }
    }
}
