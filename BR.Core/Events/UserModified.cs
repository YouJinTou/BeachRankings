using BR.Core.Abstractions;

namespace BR.Core.Events
{
    public class UserModified : EventBase
    {
        public UserModified(string streamId, int offset) 
            : base(streamId, offset)
        {
        }
    }
}
