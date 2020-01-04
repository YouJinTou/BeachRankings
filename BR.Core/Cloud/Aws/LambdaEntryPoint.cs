using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace BR.Core.Cloud.Aws
{
    public class LambdaEntryPoint
    {
    }
}
