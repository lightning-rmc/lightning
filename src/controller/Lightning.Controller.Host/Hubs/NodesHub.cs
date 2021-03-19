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
			//TODO: Try Catch
			try
			{
				await foreach (var update in _lifetimeService.GetAllNodeStatesAllAsync(Context.ConnectionAborted))
				{
					await NotifyNodeStateUpdate(update.NodeId, update.State);
				}

			}
			catch
			{

			}
		}


		public async Task NotifyNodeStateUpdate(string nodeId, NodeState command)
		{
			await Clients.Caller.SendAsync("nodeStateUpdate", nodeId, command.ToString(),Context.ConnectionAborted);
		}


		public async Task NotifyNodeConnected(string nodeId)
		{
			await Clients.Caller.SendAsync("nodeConnected", nodeId, Context.ConnectionAborted);
		}
	}
}
