using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using BR.Core.Abstractions;
using BR.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BR.Core.Cloud.Aws
{
    public class EventsServiceEntryPoint
    {
        private readonly IEventStore store;

        public EventsServiceEntryPoint()
        {
            var provider = new ServiceCollection().AddCore().BuildServiceProvider();
            this.store = provider.GetService<IEventStore>();
        }

        public EventsServiceEntryPoint(IEventStore store)
        {
            this.store = store;
        }

        public async Task HandleAsync(SNSEvent @event, ILambdaContext context)
        {
            foreach (var record in @event.Records)
            {
                await this.ProcessRecordAsync(record, context);
            }
        }

        private async Task ProcessRecordAsync(SNSEvent.SNSRecord record, ILambdaContext context)
        {
            context.Logger.LogLine($"Incoming event: {Environment.NewLine}{record.Sns.Message}");

            await this.store.AppendFromStringAsync(record.Sns.Message);
        }
    }
}
