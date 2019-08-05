using BeachRankings.Core.Extensions;
using BeachRankings.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BeachRankings.Loader.Services
{
    public static class IoC
    {
        private static readonly bool initialized;
        private static ServiceProvider provider;

        static IoC()
        {
            if (initialized)
            {
                return;
            }

            AddServices();

            initialized = true;
        }

        public static T GetService<T>()
        {
            return provider.GetService<T>();
        }

        private static void AddServices()
        {
            var services = new ServiceCollection();

            services
                .AddTransient<Settings>()
                .AddByConvention(typeof(IoC).Assembly)
                .AddDb();

            provider = services.BuildServiceProvider();
        }
    }
}
