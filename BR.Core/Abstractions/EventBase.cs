using System;

namespace BR.Core.Abstractions
{
    public abstract class EventBase
    {
        public EventBase(Guid streamId, int offset)
        {
            this.StreamId = streamId;
            this.Offset = offset;
            this.Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        public Guid StreamId { get; set; }

        public int Offset { get; set; }

        public string Type { get; set; }

        public long Timestamp { get; set; }
    }
}
