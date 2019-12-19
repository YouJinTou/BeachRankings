using BR.Core.Extensions;
using Newtonsoft.Json;
using System;
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

        public static EventStream CreateStream(params EventBase[] events)
        {
            return new EventStream(events ?? new EventBase[] { });
        }

        public bool IsEmpty()
        {
            return this.IsNullOrEmpty();
        }

        public bool ContainsEvent<T>(Func<T, bool> func)
        {
            return this.Any(e => func(JsonConvert.DeserializeObject<T>(e.Body)));
        }

        public int GetNextOffset(string type)
        {
            var leader = this
                .Where(e => e.Type == type)
                .OrderByDescending(e => e.Offset)
                .FirstOrDefault();

            return (leader == null) ? 0 : leader.Offset + 1;
        }

        public int GetNextOffset()
        {
            if (this.IsEmpty())
            {
                return 0;
            }

            return this.OrderByDescending(e => e.Offset).First().Offset + 1;
        }

        public IEnumerator<EventBase> GetEnumerator()
        {
            return this.events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.events.GetEnumerator();
        }
    }
}
