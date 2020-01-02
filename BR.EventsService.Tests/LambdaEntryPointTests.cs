using Amazon.Lambda.SNSEvents;
using Amazon.Lambda.TestUtilities;
using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Models;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BR.EventsService.Tests
{
    public class LambdaEntryPointTests
    {
        [Fact]
        public async Task PassSingleEvent()
        {
            var eventString = JsonConvert.SerializeObject(this.CreateModel());
            var snsEvent = this.CreateSnsEvent(eventString);
            var logger = new TestLambdaLogger();
            var context = new TestLambdaContext
            {
                Logger = logger
            };

            await this.CreateEntryPoint().HandleAsync(snsEvent, context);

            Assert.Contains(eventString, logger.Buffer.ToString());
        }

        [Fact]
        public async Task PassMultipleEvents()
        {
            var stream = new List<EventBase>
            {
                this.CreateModel(),
                this.CreateModel()
            };
            var eventsString = JsonConvert.SerializeObject(stream);
            var snsEvent = this.CreateSnsEvent(eventsString);
            var logger = new TestLambdaLogger();
            var context = new TestLambdaContext
            {
                Logger = logger
            };

            await this.CreateEntryPoint().HandleAsync(snsEvent, context);

            Assert.Contains(eventsString, logger.Buffer.ToString());
        }

        private LambdaEntryPoint CreateEntryPoint()
        {
            return new LambdaEntryPoint(Mock.Of<IEventStore>());
        }

        private EventBase CreateModel()
        {
            return new EventBase
            {
                StreamId = "STREAM_ID",
                Body = "BODY",
                TimeStamp = 123,
                Offset = 2,
                Type = "TYPE"
            };
        }

        private SNSEvent CreateSnsEvent(string message)
        {
            return new SNSEvent
            {
                Records = new List<SNSEvent.SNSRecord>
                {
                    new SNSEvent.SNSRecord
                    {
                        Sns = new SNSEvent.SNSMessage()
                        {
                            Message = message
                        }
                    }
                }
            };
        }
    }
}
