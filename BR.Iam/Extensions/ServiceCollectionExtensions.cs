using BR.Core.Abstractions;
using BR.Core.Extensions;
using BR.Iam.Models;
using BR.Iam.Processing;
using Microsoft.Extensions.DependencyInjection;

namespace BR.Iam.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIam(this IServiceCollection services)
        {
            services
                .AddByConvention(typeof(User).Assembly)
                .AddTransient<IStreamProjector, UserStreamProjector>();

            return services;
        }
    }
}
