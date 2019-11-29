using System;

namespace BR.Core.Events
{
    public abstract class EventBase
    {
        public EventBase(int id, string type)
        {
            this.Id = id;
            this.Type = type;
            this.Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        public int Id { get; set; }

        public string Type { get; set; }

        public long Timestamp { get; set; }
    }
}
