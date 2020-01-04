using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using BR.Core.Extensions;
using BR.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BR.Core.Cloud.Aws
{
    public class IndexServiceEntryPoint
    {
        private readonly IIndexService service;

        public IndexServiceEntryPoint()
        {
            var provider = new ServiceCollection().AddCore().BuildServiceProvider();
            this.service = provider.GetService<IIndexService>();
        }

        public IndexServiceEntryPoint(IIndexService service)
        {
            this.service = service;
        }

        public async Task HandleAsync(SNSEvent evnt, ILambdaContext context)
        {
            foreach(var record in evnt.Records)
            {
                await ProcessRecordAsync(record, context);
            }
        }

        private async Task ProcessRecordAsync(SNSEvent.SNSRecord record, ILambdaContext context)
        {
            context.Logger.LogLine($"Incoming event: {Environment.NewLine}{record.Sns.Message}");

            await this.service.UpdateIndexAsync(record.Sns.Message);
        }
    }
}
