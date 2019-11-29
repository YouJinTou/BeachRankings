using BR.Core.Abstractions;
using System;

namespace BR.Core.Events
{
    public class BeachModified : EventBase
    {
        public BeachModified(Guid id, int offset) 
            : base(id, offset)
        {
        }
    }
}
