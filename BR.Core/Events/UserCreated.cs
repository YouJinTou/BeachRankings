using BR.Core.Abstractions;

namespace BR.Core.Events
{
    public class UserCreated : EventBase
    {
        public UserCreated(string streamId, int offset) 
            : base(streamId, offset)
        {
        }
    }
}
