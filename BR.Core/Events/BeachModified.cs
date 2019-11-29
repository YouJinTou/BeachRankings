using BR.Core.Abstractions;

namespace BR.Core.Events
{
    public class BeachModified : EventBase
    {
        public BeachModified(string streamId, int offset) 
            : base(streamId, offset)
        {
        }
    }
}
