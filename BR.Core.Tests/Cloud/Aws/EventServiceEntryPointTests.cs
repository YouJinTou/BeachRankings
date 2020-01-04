using Amazon.Lambda.SNSEvents;
using Amazon.Lambda.TestUtilities;
using BR.Core.Abstractions;
using BR.Core.Cloud.Aws;
using BR.Core.Models;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BR.IndexService.Tests
{
    public class EventServiceEntryPointTests
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
            var stream = new List<AppEvent>
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

        private EventsServiceEntryPoint CreateEntryPoint()
        {
            return new EventsServiceEntryPoint(Mock.Of<IEventStore>());
        }

        private AppEvent CreateModel()
        {
            return new AppEvent
            {
                StreamId = "STREAM_ID",
                Body = "BODY",
                TimeStamp = 123,
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
