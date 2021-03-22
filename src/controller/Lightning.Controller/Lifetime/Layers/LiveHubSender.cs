using Lightning.Core.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime.Layers
{
	public class LiveHubSender : ICreateOnStartup
	{

		public LiveHubSender(IHubContext<LiveHubReceiver, ILiveHubClient> context,
			ILayerActivationService layerActivationService,
			ILogger<LiveHubSender>? logger = null)
		{
			_ = Task.Run(async () =>
			{
				await foreach (var (layerId, isActive) in layerActivationService.GetLayerActivationsAllAsync())
				{
					await context.Clients.All.LayerActivationChangedAsync(layerId, isActive);
				}
			});
		}
	}
}
