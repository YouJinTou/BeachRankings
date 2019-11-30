using BR.Core.Abstractions;
using BR.Core.Extensions;
using BR.Core.Tools;
using System;

namespace BR.Core.Events
{
    public abstract class EventBase : IDbModel
    {
        public EventBase()
        {
        }

        public EventBase(string streamId, int offset, string body = null)
        {
            this.StreamId = Validator.ReturnOrThrowIfNullOrWhiteSpace(streamId);
            this.Offset = offset;
            this.Body = body;
            this.TimeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            this.Type = this.GetType().Name;
        }

        public string StreamId { get; set; }

        public int Offset { get; set; }

        public string Type { get; set; }

        public long TimeStamp { get; set; }

        public string Body { get; set; }

        public override string ToString()
        {
            var eventString = $"{this.StreamId}/{this.Offset}/{this.Type}/";

            return eventString;
        }
    }
}
