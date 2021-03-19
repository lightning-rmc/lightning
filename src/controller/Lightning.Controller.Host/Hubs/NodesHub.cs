using Lightning.Controller.Lifetime;
using Lightning.Core.Lifetime;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.Hubs
{
	public class NodesHub : Hub
	{
		private readonly INodeLifetimeService _lifetimeService;
		private readonly ILogger<NodesHub>? _logger;

		public NodesHub(INodeLifetimeService lifetimeService, ILogger<NodesHub>? logger = null)
		{
			_lifetimeService = lifetimeService;
			_logger = logger;
		}


		public async override Task OnConnectedAsync()
		{
			await foreach (var update in _lifetimeService.GetAllNodeStatesAllAsync())
			{
				await NotifyNodeStateUpdate(update.NodeId, update.State);
			}
		}


		public async Task NotifyNodeStateUpdate(string nodeId, NodeState command)
		{
			await Clients.All.SendAsync("nodeStateUpdate", nodeId, command.ToString());
		}


		public async Task NotifyNodeConnected(string nodeId)
		{
			await Clients.All.SendAsync("nodeConnected", nodeId);
		}
	}
}
