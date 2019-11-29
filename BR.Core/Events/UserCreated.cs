using BR.Core.Abstractions;
using System;

namespace BR.Core.Events
{
    public class UserCreated : EventBase
    {
        public UserCreated(Guid id, int offset) 
            : base(id, offset)
        {
        }
    }
}
