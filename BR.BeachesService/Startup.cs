﻿using AutoMapper;
using BR.BeachesService.Extensions;
using BR.BeachesService.Models;
using BR.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BR.BeachesService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCore()
                .AddCors(o => o.AddPolicy("UIPolicy", p => p
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()))
                .AddBeachServices()
                .AddAutoMapper(typeof(Beach).Assembly)
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app
                    .UseHsts()
                    .UseHttpsRedirection();
            }

            app
                .UseCors("UIPolicy")
                .UseMvc();
        }
    }
}
