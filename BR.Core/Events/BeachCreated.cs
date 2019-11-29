using BR.Core.Abstractions;
using System;

namespace BR.Core.Events
{
    public class BeachCreated : EventBase
    {
        public BeachCreated(Guid streamId, int offset, string name)
            : base(streamId, offset)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
