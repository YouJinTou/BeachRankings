using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Models;
using BR.Core.Tools;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BR.Core.Messaging
{
    internal class EventBus : IEventBus
    {
        private readonly IAmazonSimpleNotificationService sns;
        private readonly Settings settings;
        private readonly ILogger<EventBus> logger;

        public EventBus(
            IAmazonSimpleNotificationService sns, Settings settings, ILogger<EventBus> logger)
        {
            this.sns = sns;
            this.settings = settings;
            this.logger = logger;
        }

        public async Task PublishEventAsync(AppEvent @event)
        {
            try
            {
                Validator.ThrowIfNull(@event, $"{nameof(@event)} missing.");

                var request = new PublishRequest
                {
                    TopicArn = this.settings.EventBusTopic,
                    Message = @event.ToJson()
                };
                var response = await this.sns.PublishAsync(request);
            }
            catch (System.Exception ex)
            {
                this.logger.LogError(ex, "Failed to publish event.");

                throw;
            }
        }

        public async Task PublishEventStreamAsync(EventStream stream)
        {
            foreach (var @event in stream)
            {
                await this.PublishEventAsync(@event);
            }
        }
    }
}
