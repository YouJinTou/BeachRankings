using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BR.Core.Abstractions;
using BR.Core.Extensions;
using BR.Core.Models;
using System.Collections.Generic;

namespace BR.Core.Cloud.Aws
{
    internal class EventsRepository : DynamoRepository<AppEvent>, IEventsRepository
    {
        public EventsRepository(string table)
            : base(table)
        {
        }

        protected override AppEvent ConvertTo(Document doc)
        {
            return doc.ConvertTo<AppEvent>();
        }

        protected override IEnumerable<AppEvent> ConvertItemsTo(
            IEnumerable<Dictionary<string, AttributeValue>> items)
        {
            var result = new List<AppEvent>();

            foreach (var item in items)
            {
                var @event = new AppEvent
                {
                    Body = item["Body"].S,
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
