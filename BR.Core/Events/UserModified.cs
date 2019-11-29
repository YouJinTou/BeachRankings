using BR.Core.Abstractions;
using System;

namespace BR.Core.Events
{
    public class UserModified : EventBase
    {
        public UserModified(Guid streamId, int offset) 
            : base(streamId, offset)
        {
        }
    }
}
