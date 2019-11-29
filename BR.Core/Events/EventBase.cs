using System;

namespace BR.Core.Events
{
    public abstract class EventBase
    {
        public EventBase(Guid id, int offset)
        {
            this.Id = id;
            this.Offset = offset;
            this.Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        public Guid Id { get; set; }

        public int Offset { get; set; }

        public string Type { get; set; }

        public long Timestamp { get; set; }
    }
}
