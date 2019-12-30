using AutoMapper;
using BR.Core;
using BR.Core.Extensions;
using BR.Iam.Configuration;
using BR.Iam.Extensions;
using BR.Iam.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BR.Iam
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
            var settings = new IamSettings();

            Configuration.GetSection("Settings").Bind(settings);

            services
                .AddSingleton(settings)
                .AddSingleton<Settings>(settings)
                .AddCore()
                .AddIam()
                .AddAutoMapper(typeof(User).Assembly)
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
                .AddCore()
                .UseMvc();
        }
    }
}
