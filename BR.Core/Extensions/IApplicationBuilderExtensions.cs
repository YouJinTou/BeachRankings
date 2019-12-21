using Microsoft.AspNetCore.Builder;

namespace BR.Core.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddCore(this IApplicationBuilder builder)
        {
            builder.UseCors(Constants.UICorsPolicy);

            return builder;
        }
    }
}
