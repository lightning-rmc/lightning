using Lightning.Controller.Lifetime;
using Lightning.Controller.Media;
using Lightning.Controller.Projects;
using Lightning.Core.Utils;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller
{
	public static class ControllerServiceExtensions
	{
		public static IServiceCollection AddNodeLifetime(this IServiceCollection services)
		{
			services.TryAddSingleton<NodeLifetimeService>();
			services.TryAddSingleton<INodeLifetimeService>(sp => sp.GetRequiredService<NodeLifetimeService>());
			services.TryAddSingleton<INodeLifetimeRequestResponsePublisher>(sp => sp.GetRequiredService<NodeLifetimeService>());
			return services;
		}

		public static IServiceCollection AddControllerServices(this IServiceCollection services)
		{
			services.AddNodeLifetime();
			services.TryAddSingleton<IProjectManager, ProjectManager>();
			return services;
		}

		public static IServiceCollection AddMediaServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.TryAddSingleton<MediaService>();
			services.TryAddSingleton<IMediaService>(sp => sp.GetRequiredService<MediaService>());
			services.AddCreateOnStartup(sp => sp.GetRequiredService<MediaService>());
			services.Configure<MediaSettings>(configuration.GetSection("Media"));
			return services;
		}
	}
}
