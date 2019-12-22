using BR.Core.Abstractions;
using BR.Core.Cloud.Aws;
using BR.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BR.Places.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlaces(this IServiceCollection services)
        {
            services
                .AddTransient<INoSqlRepository<Place>>(
                    sp => new DynamoRepository<Place>("Places"));

            return services;
        }
    }
}
