using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using ModernWMS.Core.Extentions;
namespace ModernWMS
{
    public class Startup
    {
        /// <summary>
        /// startup
        /// </summary>
        /// <param name="configuration">Config</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        ///  register service 
        /// </summary>
        /// <param name="services">services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddExtensionsService(Configuration);
        }

        /// <summary>
        /// configure
        /// </summary>
        /// <param name="app">app</param>
        /// <param name="env">env</param>
        /// <param name="serviceProvider">serviceProvider</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service_provider)
        {
            app.UseExtensionsConfigure(env, service_provider, Configuration);
        }
    }
}
