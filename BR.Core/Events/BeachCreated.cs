using System;

namespace BR.Core.Events
{
    public class BeachCreated : EventBase
    {
        public BeachCreated(Guid id, int offset, string type, string name)
            : base(id, offset, type)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
