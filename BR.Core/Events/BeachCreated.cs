using BR.Core.Abstractions;
using System;

namespace BR.Core.Events
{
    public class BeachCreated : EventBase
    {
        public BeachCreated(Guid id, int offset, string name)
            : base(id, offset)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
