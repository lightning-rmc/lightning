using Lightning.Controller.Lifetime.Controller;
using Lightning.Controller.Lifetime.Layers;
using Lightning.Controller.Lifetime.Nodes;
using Lightning.Core.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lightning.Controller.Lifetime
{
	public static class LifetimeServiceExtensions
	{

		public static IServiceCollection AddControllerLifetimeServices(this IServiceCollection services)
		{
			services.AddControllerStateServices();
			services.AddNodeStateServices();
			services.AddLiveStateServices();
			return services;
		}

		public static IServiceCollection AddControllerStateServices(this IServiceCollection services)
		{
			services.TryAddSingleton<ControllerStateHandler>();
			services.TryAddSingleton<IControllerStateNotifier>(p => p.GetRequiredService<ControllerStateHandler>());
			services.TryAddSingleton<IControllerStateReceiver>(p => p.GetRequiredService<ControllerStateHandler>());
			services.AddHostedService<ControllerStateBootstrapper>();
			services.AddCreateOnStartup<ControllerHubSender>();
			return services;
		}

		public static IServiceCollection AddNodeStateServices(this IServiceCollection services)
		{
			services.TryAddSingleton<NodeLifetimeService>();
			services.TryAddSingleton<INodeLifetimeService>(sp => sp.GetRequiredService<NodeLifetimeService>());
			services.TryAddSingleton<INodeLifetimeRequestResponsePublisher>(sp => sp.GetRequiredService<NodeLifetimeService>());
			services.AddCreateOnStartup<NodesHubSender>();
			return services;
		}

		public static IServiceCollection AddLiveStateServices(this IServiceCollection services)
		{
			services.TryAddSingleton<ILayerActivationService, LayerActivationService>();
			services.AddCreateOnStartup<LiveHubSender>();
			return services;
		}
	}
}
