using System;

namespace BR.Core.Events
{
    public abstract class EventBase
    {
        public EventBase(Guid id, int offset, string type)
        {
            this.Id = id;
            this.Offset = offset;
            this.Type = type;
            this.Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        public Guid Id { get; set; }

        public int Offset { get; set; }

        public string Type { get; set; }

        public long Timestamp { get; set; }
    }
}
