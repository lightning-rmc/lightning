using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lightning.Controller.Host.Hubs;
using Lightning.Controller.Lifetime;
using Lightning.Controller.Media;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lightning.Controller.Host
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
			services.AddSignalR();
			services.AddCors();
			services.AddControllerServices();
			services.AddControllers();
			services.AddGrpc();
			services.AddNodeLifetime();
			services.AddMediaServices(_configuration);
		}


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
				app.UseCors(builder =>
				{
					builder.WithOrigins("http://localhost:4200")
						.AllowAnyHeader()
						.AllowAnyMethod()
						.AllowCredentials();
				});
			}


			app.UseRouting();
			//app.UseAuthentication();
			//app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
				//Grpc Services
				endpoints.MapGrpcService<GrpcLifetimeService>();

				//API Services
				endpoints.MapControllers();

				//SignalR Services
				endpoints.MapHub<NodesHub>("/hubs/nodes");

			});
        }
    }
}
