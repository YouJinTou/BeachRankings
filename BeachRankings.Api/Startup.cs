using AutoMapper;
using BeachRankings.Api.Models.Beaches;
using BeachRankings.Core.Extensions;
using BeachRankings.Core.Models;
using BeachRankings.Core.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BeachRankings.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            var assemblies = new Assembly[] 
            {
                typeof(InputValidator).Assembly,
                typeof(BeachViewModel).Assembly
            };

            services
                .AddTransient<Settings>()
                .AddByConvention(assemblies)
                .AddAutoMapper(assemblies)
                .AddDb()
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors(p => p.AllowAnyOrigin());
            app.UseMvc();
        }
    }
}
