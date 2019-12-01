using BR.Core.Abstractions;
using BR.Core.Tools;
using Newtonsoft.Json;
using System;

namespace BR.Core.Events
{
    public abstract class EventBase : IDbModel
    {
        public EventBase()
        {
        }

        public EventBase(string streamId, int offset, object body = null)
        {
            this.StreamId = Validator.ReturnOrThrowIfNullOrWhiteSpace(streamId);
            this.Offset = offset;
            this.Body = (body == null) ? null : JsonConvert.SerializeObject(body);
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
