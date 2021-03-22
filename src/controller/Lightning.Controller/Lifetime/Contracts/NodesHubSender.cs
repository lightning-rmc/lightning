using Lightning.Core.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime.Contracts
{
	public class NodesHubSender : ICreateOnStartup
	{
		public NodesHubSender(IHubContext<NodesHubReceiver, INodeHubClient> hubContext, INodeLifetimeService nodeLifetimeService, ILogger<NodesHubSender>? logger = null)
		{
			_ = Task.Run(async () =>
			{
				await foreach (var (nodeId, state) in nodeLifetimeService.GetAllNodeStatesAllAsync())
				{
					logger?.LogTrace("Send node state '{id}:{state}' to all signalR clients", nodeId, state);
					await hubContext.Clients.All.NodeStateUpdateAsync(nodeId, state.ToString());
				}
			});
		}
	}
}
