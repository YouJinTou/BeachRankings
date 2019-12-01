using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Extensions;
using System.Collections.Generic;

namespace BR.Core.Cloud.Aws
{
    internal class EventsRepository : DynamoRepository<EventBase>, IEventsRepository
    {
        public EventsRepository(string table)
            : base(table)
        {
        }

        protected override EventBase ConvertTo(Document doc)
        {
            return doc.ConvertTo<GenericEvent>();
        }

        protected override IEnumerable<EventBase> ConvertItemsTo(
            IEnumerable<Dictionary<string, AttributeValue>> items)
        {
            var result = new List<EventBase>();

            foreach (var item in items)
            {
                var @event = new GenericEvent
                {
                    Body = item["Body"].S,
                    Offset = int.Parse(item["Offset"].N),
                    StreamId = item[Constants.StreamId].S,
                    TimeStamp = long.Parse(item["TimeStamp"].N),
                    Type = item["Type"].S
                };

                result.Add(@event);
            }

            return result;
        }
    }
}
