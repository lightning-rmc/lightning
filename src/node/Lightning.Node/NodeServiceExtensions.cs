using Lightning.Core;
using Lightning.Core.Media;
using Lightning.Core.Rendering;
using Lightning.Core.Utils;
using Lightning.Node.Communications;
using Lightning.Node.Lifetime;
using Lightning.Node.Media;
using Lightning.Node.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenCvSharp;
using System;

namespace Lightning.Node
{
	public static class NodeServiceExtensions
	{
		public static IServiceCollection AddNodeServices(this IServiceCollection services, IConfiguration configuration)
		{
			if (services is null)
				throw new ArgumentNullException(nameof(services));

			services.TryAddSingleton<OpenCVWindowHost>();
			services.TryAddSingleton<IWindowHost<Mat>>(p => p.GetRequiredService<OpenCVWindowHost>());
			services.AddCreateOnStartup(p => p.GetRequiredService<OpenCVWindowHost>());
			services.AddNodeRendering();
			services.AddOpenCVWindowHost();
			services.AddFeatureFlags(configuration);
			services.AddNodeConfiguration(configuration);
			services.AddCreateOnStartup<GrpcNodeLifetimeService>();
			services.AddCreateOnStartup<GrpcNodeMediaSyncService>();
			services.TryAddSingleton<NodeCommandHandler>();
			services.TryAddSingleton<INodeStateReceiver>(p => p.GetRequiredService<NodeCommandHandler>());
			services.TryAddSingleton<INodeStateNotifier>(p => p.GetRequiredService<NodeCommandHandler>());
			services.AddHostedService<NodeStateBootstrapper>();
			//TODO: Refactor find right place
			services.TryAddSingleton<IMediaResolver, NodeMediaResolver>();
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
