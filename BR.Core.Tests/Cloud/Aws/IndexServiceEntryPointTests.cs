using Amazon.Lambda.SNSEvents;
using Amazon.Lambda.TestUtilities;
using BR.Core.Abstractions;
using BR.Core.Cloud.Aws;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BR.Core.Tests.Cloud.Aws
{
    public class IndexServiceEntryPointTests
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
            var function = new IndexServiceEntryPoint(Mock.Of<IIndexService>());

            await function.HandleAsync(snsEvent, context);

            Assert.Contains("Incoming event", logger.Buffer.ToString());
        }
    }
}
