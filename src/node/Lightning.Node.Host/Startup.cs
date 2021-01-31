using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lightning.Core;
using Lightning.Core.Utils;
using Lightning.Node.Communications;
using Lightning.Node.Lifetime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lightning.Node.Host
{
    public class Startup
    {
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

        public void ConfigureServices(IServiceCollection services)
        {
			services.AddRendering();
			services.AddOpenCVWindowHost();
			services.AddHostedService<NodeBootStrapper>();
			services.AddFeatureFlgs(_configuration);
			services.AddCreateOnStarup<NodeLifetimeController>();
			services.AddSingleton<IConnectionResolver, ConfigurationConnectionResolver>();
			services.AddSingleton<IGrpcConnectionManager, GrpcConnectionManager>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
