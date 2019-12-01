using BR.BeachesService.Models;
using BR.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace BR.BeachesService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBeachServices(this IServiceCollection services)
        {
            services
                .AddByConvention(typeof(Beach).Assembly);

            return services;
        }
    }
}
