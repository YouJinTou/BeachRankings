using BR.Core.Abstractions;
using BR.Core.Cloud.Aws;
using BR.Core.Extensions;
using BR.Iam.Configuration;
using BR.Iam.Models;
using BR.Iam.Processing;
using Microsoft.Extensions.DependencyInjection;

namespace BR.Iam.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIam(this IServiceCollection services)
        {
            var settings = services.BuildServiceProvider().GetService<IamSettings>();

            services
                .AddByConvention(typeof(User).Assembly)
                .AddTransient<INoSqlRepository<User>>(
                    sp => new DynamoRepository<User>(settings.UsersTable))
                .AddTransient<IStreamProjector, UserStreamProjector>();

            return services;
        }
    }
}
