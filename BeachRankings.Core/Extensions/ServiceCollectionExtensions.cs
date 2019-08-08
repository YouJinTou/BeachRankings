using Amazon.DynamoDBv2;
using BeachRankings.Core.Abstractions;
using BeachRankings.Core.DAL;
using BeachRankings.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace BeachRankings.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddByConvention(
            this IServiceCollection services, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var serviceItems = assembly.GetTypes()
                    .Where(t =>
                        t.IsClass &&
                        !t.IsAbstract &&
                        !t.IsGenericType &&
                        t.GetInterfaces().Any(i => $"I{t.Name}" == i.Name))
                    .Select(t => new
                    {
                        Implementation = t,
                        Interface = t.GetInterface($"I{t.Name}")
                    })
                    .ToList();

                foreach (var service in serviceItems)
                {
                    services.AddTransient(service.Interface, service.Implementation);
                }
            }

            return services;
        }

        public static IServiceCollection AddDb(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var settings = provider.GetService<Settings>();

            services.AddSingleton<IAmazonDynamoDB>(sp => new AmazonDynamoDBClient(
                settings.AwsAccessToken, 
                settings.AwsSecret, 
                settings.AwsRegion.ToRegionEndpoint()));

            provider = services.BuildServiceProvider();
            var dynamoClient = provider.GetService<IAmazonDynamoDB>();

            services
                .AddTransient<IRepository<Location>>(
                    sp => new DynamoRepository<Location>(dynamoClient, "Locations"))
                .AddTransient<IRepository<Beach>>(
                    sp => new DynamoRepository<Beach>(dynamoClient, "Beaches"))
                .AddTransient<IRepository<Review>>(
                    sp => new DynamoRepository<Review>(dynamoClient, "Reviews"))
                .AddTransient<IBeachesRepository>(
                    sp => new BeachesRepository(dynamoClient, "Beaches"));

            return services;
        }
    }
}
