using Lightning.Controller.Lifetime;
using Lightning.Core.Lifetime;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.Hubs
{
	public class NodeHub : Hub<INodeHubClient>
	{
		private readonly INodeLifetimeService _lifetimeService;
		private readonly ILogger<NodeHub>? _logger;

		public NodeHub(INodeLifetimeService lifetimeService, ILogger<NodeHub>? logger = null)
		{
			_lifetimeService = lifetimeService;
			_logger = logger;
		}


		public override Task OnConnectedAsync()
		{
			Task.Run(async () =>
			{
				await foreach (var update in _lifetimeService.GetAllNodeStatesAllAsync())
				{
					await NotifyNodeStateUpdate(update.NodeId, update.State);
				}
			});

			//TODO: Handle new node connections

			return Task.CompletedTask;
		}


		public async Task NotifyNodeStateUpdate(string nodeId, NodeState command)
		{
			await Clients.All.NodeStateUpdated(nodeId, command.ToString());
		}


		public async Task NotifyNodeConnected(string nodeId)
		{
			await Clients.All.NodeConnected(nodeId);
		}
	}
}
