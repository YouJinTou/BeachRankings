using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using BR.Core.Extensions;
using BR.IndexService.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace BR.IndexService
{
    public class LambdaEntryPoint
    {
        private readonly IIndexService service;

        public LambdaEntryPoint()
        {
            var services = new ServiceCollection()
                .AddCore()
                .AddByConvention(typeof(IIndexService).Assembly);
            var provider = services.BuildServiceProvider();
            this.service = provider.GetService<IIndexService>();
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

            await Task.CompletedTask;
        }
    }
}
