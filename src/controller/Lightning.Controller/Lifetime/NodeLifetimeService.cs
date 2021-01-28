using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	internal class NodeLifetimeService : INodeLifetimeService
	{
		private readonly Dictionary<string, Channel<NodeState>> _nodeChannels;
		private readonly Dictionary<string, NodeState> _nodes;
		private readonly ILogger<NodeLifetimeService>? _logger;
		private readonly Channel<(string nodeId, NodeState state)> _allUpdatesChannel;


		public NodeLifetimeService(ILogger<NodeLifetimeService>? logger = null)
		{
			_nodeChannels = new Dictionary<string, Channel<NodeState>>();
			_nodes = new Dictionary<string, NodeState>();
			_allUpdatesChannel = Channel.CreateUnbounded<(string, NodeState)>();
		}

		public bool TryRegisterNode(string nodeId)
		{
			if (_nodes.ContainsKey(nodeId))
			{
				return false;
			}
			_nodes.Add(nodeId, NodeState.Offline);
			_nodeChannels.Add(nodeId, Channel.CreateUnbounded<NodeState>());
			_logger?.LogInformation("Register new Node with id:'{nodeId}'.", nodeId);
			return true;
		}

		public IAsyncEnumerable<(string NodeId, NodeState State)> GetAllNodeStatesAllAsync()
			=> _allUpdatesChannel.Reader.ReadAllAsync();

		public IAsyncEnumerable<NodeState> GetNodeStatesAllAsync(string nodeId)
		{
			if (_nodeChannels.ContainsKey(nodeId))
			{
				return _nodeChannels[nodeId].Reader.ReadAllAsync();
			}
			throw new KeyNotFoundException($"{nameof(nodeId)}: '{nodeId}'");
		}

		public async Task UpdateNodeStateAsync(NodeState state, string? nodeId = null)
		{
			if (nodeId is not null)
			{
				if (!_nodes.ContainsKey(nodeId))
				{
					throw new KeyNotFoundException($"{nameof(nodeId)}: '{nodeId}'");
				}

				_nodes[nodeId] = state;
				await _nodeChannels[nodeId].Writer.WriteAsync(state);
				await _allUpdatesChannel.Writer.WriteAsync((nodeId, state));
				_logger?.LogDebug("NodeState from node '{nodeId}' changed to '{state}'", nodeId, state);
			}
			else
			{
				foreach (var node in _nodes.Keys)
				{
					await UpdateNodeStateAsync(state, node);
				}
			}
		}


		public IEnumerable<(string NodeId, NodeState State)> GetAllNodeStates()
			=> _nodes.Select(ns => (ns.Key, ns.Value));

		public bool RemoveNode(string nodeId)
		{
			//TODO: Not sure if needed?
			//Note: Remove also from ProjectManager
			throw new System.NotImplementedException();
		}
	}
}
