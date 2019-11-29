using BR.Core.Tools;
using System;

namespace BR.Core.Abstractions
{
    public abstract class EventBase
    {
        public EventBase(string streamId, int offset)
        {
            this.StreamId = Validator.ReturnOrThrowIfNullOrWhiteSpace(streamId);
            this.Offset = offset;
            this.Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        public string StreamId { get; set; }

        public int Offset { get; set; }

        public string Type { get; set; }

        public long Timestamp { get; set; }
    }
}
