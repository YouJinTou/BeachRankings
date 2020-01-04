using BR.Core.Extensions;
using BR.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BR.Core.Events
{
    public class EventStream : IEnumerable<AppEvent>
    {
        private IEnumerable<AppEvent> events;

        public EventStream(IEnumerable<AppEvent> events)
        {
            this.events = events ?? new List<AppEvent>();
        }

        public static EventStream CreateStream(params AppEvent[] events)
        {
            return new EventStream(events ?? new AppEvent[] { });
        }

        public bool IsEmpty()
        {
            return this.IsNullOrEmpty();
        }

        public bool ContainsEvent<T>(Func<T, bool> func)
        {
            return this.Any(e => func(JsonConvert.DeserializeObject<T>(e.Body)));
        }

        public IEnumerator<AppEvent> GetEnumerator()
        {
            return this.events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.events.GetEnumerator();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var result = this.events.Select(e => e.ToString());

            return string.Join("  ", result);
        }
    }
}
