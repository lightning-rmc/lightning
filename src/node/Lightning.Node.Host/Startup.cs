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
		private readonly FeatureFlags _featureFlags;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
			_featureFlags = configuration
							.GetSection("Featureflags")
							.Get<FeatureFlags>();
		}

        public void ConfigureServices(IServiceCollection services)
        {
			services.AddNodeCoreServices(_configuration);
			if (_featureFlags.NodeWithoutServer)
			{

			}
			else
			{

			}
			services.AddHostedService<NodeBootStrapper>();
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
