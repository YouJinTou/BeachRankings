using Amazon.SimpleNotificationService;
using BR.Core.Abstractions;
using BR.Core.Cloud.Aws;
using BR.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace BR.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services
                .AddByConvention(typeof(Constants).Assembly)
                .AddLogging()
                .AddAWSService<IAmazonSimpleNotificationService>()
                .AddUiCors()
                .AddDb();

            return services;
        }

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

        private static IServiceCollection AddDb(this IServiceCollection services)
        {
            services
                .AddTransient<INoSqlRepository<EventBase>>(
                    sp => new DynamoRepository<EventBase>("EventLog"))
                .AddTransient<IEventsRepository>(
                    sp => new EventsRepository("EventLog"));

            return services;
        }

        private static IServiceCollection AddUiCors(this IServiceCollection services)
        {
            services
                .AddCors(o => o.AddPolicy("UIPolicy", p => p
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()));

            return services;
        }
    }
}
