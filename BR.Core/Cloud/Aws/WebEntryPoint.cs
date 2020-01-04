using Amazon.Lambda.AspNetCoreServer;
using BR.Services;
using Microsoft.AspNetCore.Hosting;

namespace BR.Core.Cloud.Aws
{
    public class WebEntryPoint : APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();
        }
    }
}
