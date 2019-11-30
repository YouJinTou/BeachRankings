using Amazon.DynamoDBv2;
using BR.Core;
using BR.Core.Abstractions;
using BR.Core.Cloud.Aws;
using BR.Core.Extensions;
using BR.Core.Helpers;
using BR.Iam.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BR.Iam.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIam(this IServiceCollection services)
        {
            services
                .AddByConvention(typeof(User).Assembly)
                .AddDb();

            return services;
        }

        private static IServiceCollection AddDb(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            services.AddSingleton<IAmazonDynamoDB>(sp => new AmazonDynamoDBClient());

            provider = services.BuildServiceProvider();
            var dynamoClient = provider.GetService<IAmazonDynamoDB>();

            services.AddTransient<INoSqlRepository<User>>(
                sp => new DynamoRepository<User>(dynamoClient, $"Users_{EnvHelper.Stage}"));

            return services;
        }
    }
}
