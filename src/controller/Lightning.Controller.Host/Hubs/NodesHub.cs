using Lightning.Controller.Lifetime;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.Hubs
{
	public class NodesHub : Hub
	{
		private readonly INodeLifetimeService _lifetimeService;

		public NodesHub(INodeLifetimeService lifetimeService)
		{
			_lifetimeService = lifetimeService;

		}

		private async Task RegisterUpdates()
		{

		}

		public async Task NotifyNodeConnected(string nodeId)
		{
			await Clients.All.SendAsync("nodeConnected", nodeId);
		}
	}
}
