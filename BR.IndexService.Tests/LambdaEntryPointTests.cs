using Amazon.Lambda.SNSEvents;
using Amazon.Lambda.TestUtilities;
using BR.IndexService.Abstractions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BR.IndexService.Tests
{
    public class LambdaEntryPointTests
    {
        [Fact]
        public async Task TestSQSEventLambdaFunction()
        {
            var snsEvent = new SNSEvent
            {
                Records = new List<SNSEvent.SNSRecord>
                {
                    new SNSEvent.SNSRecord
                    {
                        Sns = new SNSEvent.SNSMessage()
                        {
                            Message = "foobar"
                        }
                    }
                }
            };
            var logger = new TestLambdaLogger();
            var context = new TestLambdaContext
            {
                Logger = logger
            };
            var function = new LambdaEntryPoint(Mock.Of<IIndexService>());

            await function.HandleAsync(snsEvent, context);

            Assert.Contains("Incoming event", logger.Buffer.ToString());
        }
    }
}
