using System;

namespace BR.Core.Events
{
    public class BeachModified : EventBase
    {
        public BeachModified(Guid id, int offset, string type) 
            : base(id, offset, type)
        {
        }
    }
}
