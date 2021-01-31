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
			RegisterUpdates();
		}

		private async Task RegisterUpdates()
		{
			await foreach (var update in _lifetimeService.GetAllNodeStatesAllAsync())
			{
				await NotifyNodeStateUpdate(update.NodeId, update.State);
			}
		}


		public async Task NotifyNodeStateUpdate(string nodeId, NodeState state)
		{
			await Clients.All.SendAsync("nodeStateUpdate", nodeId, state);
		}

		public async Task NotifyNodeConnected(string nodeId)
		{
			await Clients.All.SendAsync("nodeConnected", nodeId);
		}

		public async Task RequestMessage()
		{
			await Task.Delay(1000);
			await NotifyNodeStateUpdate("test", NodeState.Info);
		}
	}
}
