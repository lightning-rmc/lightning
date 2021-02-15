using Lightning.Core;
using Lightning.Core.Rendering;
using Lightning.Core.Utils;
using Lightning.Node.Communications;
using Lightning.Node.Lifetime;
using Lightning.Node.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node
{
	public static class NodeServiceExtensions
	{
		public static IServiceCollection AddNodeServices(this IServiceCollection services, IConfiguration configuration)
		{
			if (services is null)
				throw new ArgumentNullException(nameof(services));

			services.AddNodeRendering();
			services.AddOpenCVWindowHost();
			services.AddFeatureFlags(configuration);
			services.AddNodeConfiguration(configuration);
			services.AddCreateOnStartup<GrpcNodeLifetimeService>();
			return services;
		}

		public static IServiceCollection AddNodeConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<NodeConfiguration>(configuration.GetSection("Node"));
			return services;
		}

		public static IServiceCollection AddGrpcRemoteServices(this IServiceCollection services)
		{
			if (services is null)
				throw new ArgumentNullException(nameof(services));

			services.AddSingleton<IConnectionResolver, ConfigurationConnectionResolver>();
			services.AddSingleton<IConnectionManager, ConnectionManager>();
			return services;
		}

		public static IServiceCollection AddNodeRendering(this IServiceCollection services)
		{
			if (services is null)
				throw new ArgumentNullException(nameof(services));

			services.AddRenderingCore();
			services.TryAddSingleton<IRenderTreeBuilder<Mat>,NodeRenderTreeBuilder>();
			return services;
		}
	}
}
