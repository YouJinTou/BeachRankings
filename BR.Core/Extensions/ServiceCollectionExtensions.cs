using Amazon.SimpleNotificationService;
using AutoMapper;
using BR.Core.Abstractions;
using BR.Core.Cloud.Aws;
using BR.Core.Models;
using BR.Core.Processing;
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
                .AddAutoMapper(typeof(Constants).Assembly)
                .AddProjectors()
                .AddDb();

            return services;
        }

        private static IServiceCollection AddByConvention(
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
            var settings = services.BuildServiceProvider().GetService<Settings>() 
                ?? new Settings();

            services
                .AddTransient<INoSqlRepository<AppEvent>>(
                    sp => new DynamoRepository<AppEvent>(settings.EventLogTable))
                 .AddTransient<IEventsRepository>(
                    sp => new EventsRepository(settings.EventLogTable))
                .AddTransient<INoSqlRepository<Place>>(
                    sp => new DynamoRepository<Place>(settings.PlacesTable))
                .AddTransient<INoSqlRepository<User>>(
                    sp => new DynamoRepository<User>(settings.UsersTable))
                .AddTransient<INoSqlRepository<IndexEntry>>(
                    sp => new DynamoRepository<IndexEntry>(settings.IndexTable));

            return services;
        }

        private static IServiceCollection AddProjectors(this IServiceCollection services)
        {
            services
                .AddTransient<IReviewStreamProjector, ReviewStreamProjector>()
                .AddTransient<IBeachStreamProjector, BeachStreamProjector>()
                .AddTransient<IUserStreamProjector, UserStreamProjector>();

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
