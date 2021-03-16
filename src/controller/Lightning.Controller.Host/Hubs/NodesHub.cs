using Lightning.Controller.Lifetime;
using Lightning.Core.Lifetime;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Lightning.Controller.Host.Hubs
{
	public class NodesHub : Hub
	{
		private readonly INodeLifetimeService _lifetimeService;

		public NodesHub(INodeLifetimeService lifetimeService)
		{
			_lifetimeService = lifetimeService;
		}


		public async override Task OnConnectedAsync()
		{
			await foreach (var (NodeId, Command) in _lifetimeService.GetAllNodeStatsAllAsync())
			{
				await NotifyNodeStateUpdate(NodeId, Command);
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

		public async Task RequestMessage()
		{
			await Task.Delay(1000);
		}
	}
}
