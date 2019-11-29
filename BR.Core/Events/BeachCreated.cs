using BR.Core.Abstractions;

namespace BR.Core.Events
{
    public class BeachCreated : EventBase
    {
        public BeachCreated(string streamId, int offset, string name)
            : base(streamId, offset)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
