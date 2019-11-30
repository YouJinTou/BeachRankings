using BR.Core.Abstractions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BR.Core.Events
{
    public class EventStream : IEnumerable<EventBase>
    {
        private IEnumerable<EventBase> events;

        public EventStream(IEnumerable<EventBase> events, int readOffset = 0)
        {
            this.events = events ?? new List<EventBase>();
            this.events = this.events.Where(e => e.Offset >= readOffset);
        }

        public IEnumerator<EventBase> GetEnumerator()
        {
            return this.events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
