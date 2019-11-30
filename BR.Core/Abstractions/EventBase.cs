﻿using BR.Core.Events;
using BR.Core.Extensions;
using BR.Core.Tools;
using System;

namespace BR.Core.Abstractions
{
    public abstract class EventBase : IDbModel
    {
        public EventBase(string streamId, int offset, string body = null)
        {
            this.StreamId = Validator.ReturnOrThrowIfNullOrWhiteSpace(streamId);
            this.Offset = offset;
            this.Body = body;
            this.Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            this.Type = this.GetType().Name;
        }

        public string StreamId { get; set; }

        public int Offset { get; set; }

        public string Type { get; set; }

        public long Timestamp { get; set; }

        public string Body { get; set; }

        public EventStream ToEventStream()
        {
            return new EventStream(this.AsEnumerable());
        }
    }
}
