﻿using BR.BeachesService.Models;
using BR.BeachesService.Processing;
using BR.Core.Abstractions;
using BR.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace BR.BeachesService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBeachServices(this IServiceCollection services)
        {
            services
                .AddByConvention(typeof(Beach).Assembly)
                .AddTransient<IStreamProjector, BeachStreamProjector>();

            return services;
        }
    }
}