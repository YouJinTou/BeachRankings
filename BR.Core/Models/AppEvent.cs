using BR.Core.Abstractions;
using BR.Core.Tools;
using Newtonsoft.Json;
using System;

namespace BR.Core.Models
{
    public class AppEvent : IDbModel
    {
        public AppEvent()
        {
        }

        public AppEvent(string streamId, object body, string type)
        {
            this.StreamId = Validator.ReturnOrThrowIfNullOrWhiteSpace(streamId);
            this.Body = JsonConvert.SerializeObject(body);
            this.TimeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            this.Type = Validator.ReturnOrThrowIfNullOrWhiteSpace(type);
        }

        public string StreamId { get; set; }

        public long TimeStamp { get; set; }

        public string Type { get; set; }

        public string Body { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override string ToString()
        {
            var eventString = $"{this.StreamId}/{this.Type}/";

            return eventString;
        }
    }
}
