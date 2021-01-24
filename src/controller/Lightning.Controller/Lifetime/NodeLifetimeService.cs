using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public class NodeLifetimeService : INodeLifetimeService
	{
		private readonly Dictionary<string, Channel<NodeState>> _nodeChannels;
		private readonly ILogger<NodeLifetimeService>? _logger;

		public NodeLifetimeService(ILogger<NodeLifetimeService>? logger = null)
		{
			_nodeChannels = new Dictionary<string, Channel<NodeState>>();
			_logger = logger;
		}

		public bool AuthenticateNode(string nodeId)
		{
			throw new System.NotImplementedException();
		}

		public IAsyncEnumerable<NodeState> GetNodeStatesAllAsync(string nodeId)
		{
			//throw new System.NotImplementedException();
			return null!;
		}

		public Task UpdateNodeStateAsync(NodeState state, string? nodeId = null)
		{
			return Task.CompletedTask;
		}
	}
}
