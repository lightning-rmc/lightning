using Lightning.Controller.Lifetime.Controller;
using Lightning.Controller.Lifetime.Layers;
using Lightning.Controller.Lifetime.Nodes;
using Lightning.Controller.Media;
using Lightning.Controller.Projects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

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
			services.AddControllerServices(_configuration);
			services.AddSignalR();
			services.AddCors();
			services.AddControllers();
			services.AddGrpc();
			services.Configure<HostOptions>(
						opts => opts.ShutdownTimeout = TimeSpan.FromSeconds(15));
		}


		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<MediaSettings> mediaSettings)
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

			app.UseFileServer(new FileServerOptions()
			{
				FileProvider = new PhysicalFileProvider(mediaSettings.Value.StoragePath),
				RequestPath = "/media",
				EnableDirectoryBrowsing = true
			});

			app.UseEndpoints(endpoints =>
			{
				//Grpc Services
				endpoints.MapGrpcService<GrpcNodeLifetimeService>();
				endpoints.MapGrpcService<GrpcConfigurationService>();
				endpoints.MapGrpcService<GrpcMediaService>();

				//API Services
				endpoints.MapControllers();

				//SignalR Services
				endpoints.MapHub<ControllerHubReceiver>("/hubs/controller");
				endpoints.MapHub<NodesHubReceiver>("/hubs/nodes");
				endpoints.MapHub<LiveHubReceiver>("/hubs/live");
			});
		}
	}
}
