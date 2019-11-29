using BR.Core.Abstractions;
using System;

namespace BR.Core.Events
{
    public class UserModified : EventBase
    {
        public UserModified(Guid id, int offset) 
            : base(id, offset)
        {
        }
    }
}
