using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace BR.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddByConvention(typeof(Constants).Assembly);

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
    }
}
