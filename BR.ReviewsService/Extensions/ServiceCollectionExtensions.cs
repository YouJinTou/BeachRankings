using BR.ReviewsService.Models;
using BR.ReviewsService.Processing;
using BR.Core.Abstractions;
using BR.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace BR.ReviewsService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReviewServices(this IServiceCollection services)
        {
            services
                .AddByConvention(typeof(Review).Assembly)
                .AddTransient<IStreamProjector, ReviewStreamProjector>();

            return services;
        }
    }
}
