using Lightning.Controller.Lifetime;
using Lightning.Core.Lifetime;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime.Nodes
{
	public class NodesHubReceiver : Hub<INodeHubClient>, INodeHubServer
	{
		private readonly INodeLifetimeService _lifetimeService;
		private readonly ILogger<NodesHubReceiver>? _logger;

		public NodesHubReceiver(INodeLifetimeService lifetimeService, ILogger<NodesHubReceiver>? logger = null)
		{
			_lifetimeService = lifetimeService;
			_logger = logger;
		}
	}
}
