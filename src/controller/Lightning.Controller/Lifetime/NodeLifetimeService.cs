using Lightning.Controller.Projects;
using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	internal class NodeLifetimeService : INodeLifetimeService, INodeLifetimeRequestResponsePublisher
	{
		private readonly ILogger<NodeLifetimeService>? _logger;
		//TODO: maybe change to ConcurrentDictionary
		private readonly Dictionary<string, Channel<NodeState>> _nodeStateResponseChannels;
		private readonly Dictionary<string, Channel<NodeState>> _nodeStateRequestChannels;
		private readonly Dictionary<string, NodeState> _nodeStates;
		private readonly Channel<(string nodeId, NodeState state)> _allUpdatesChannel;
		private readonly IProjectManager _projectManager;


		public NodeLifetimeService(IProjectManager projectManager, ILogger<NodeLifetimeService>? logger = null)
		{
			_projectManager = projectManager;
			_nodeStateResponseChannels = new Dictionary<string, Channel<NodeState>>();
			_nodeStateRequestChannels = new Dictionary<string, Channel<NodeState>>();
			_allUpdatesChannel = Channel.CreateUnbounded<(string, NodeState)>();
			_nodeStates = new Dictionary<string, NodeState>();
			_logger = logger;

			_projectManager.ProjectLoaded += (s, e) =>
			{
				ClearAllNodes();
				var nodes = _projectManager.GetNodes();
				foreach (var node in nodes)
				{
					TryRegisterNode(node.Id);
				}
			};
		}
		public IAsyncEnumerable<(string NodeId, NodeState State)> GetAllNodeStatsAllAsync(CancellationToken token = default) =>
			_allUpdatesChannel.Reader.ReadAllAsync(token);
		public IAsyncEnumerable<NodeState> GetNodeStatsAllAsync(string nodeId, CancellationToken token = default)
		{
			if (_nodeStateResponseChannels.TryGetValue(nodeId, out var channel))
			{
				return channel.Reader.ReadAllAsync(token);
			}
			throw new KeyNotFoundException($"{nameof(nodeId)}: '{nodeId}'");
		}

		public IEnumerable<(string NodeId, NodeState State)> GetAllNodeStates()
			=> _nodeStates.Select(kv => (kv.Key, kv.Value));

		public bool TryRemoveNode(string nodeId)
		{
			if (_nodeStateRequestChannels.TryGetValue(nodeId, out var requestsChannel))
			{
				requestsChannel.Writer.TryComplete();
				_nodeStateRequestChannels.Remove(nodeId);
			}
			if (_nodeStateResponseChannels.TryGetValue(nodeId, out var responseChannel))
			{
				responseChannel.Writer.TryComplete();
				_nodeStateResponseChannels.Remove(nodeId);
			}
			_nodeStates.Remove(nodeId);
			return true;
		}

		private void ClearAllNodes()
		{
			foreach (var item in _nodeStateRequestChannels)
			{
				item.Value.Writer.TryComplete();
			}
			foreach (var item in _nodeStateResponseChannels)
			{
				item.Value.Writer.TryComplete();
			}
			_nodeStateRequestChannels.Clear();
			_nodeStateResponseChannels.Clear();
			_nodeStates.Clear();
		}

		public void TryRegisterNode(string nodeId)
		{
			if (!_nodeStates.ContainsKey(nodeId))
			{
				_nodeStates.Add(nodeId, NodeState.Offline);
				_nodeStateRequestChannels.Add(nodeId, Channel.CreateUnbounded<NodeState>());
				_nodeStateResponseChannels.Add(nodeId, Channel.CreateUnbounded<NodeState>());
				_logger?.LogInformation("Register new Node with id:'{nodeId}'.", nodeId);
			}
			else
			{
				//TODO: Add logging
			}
		}

		public async Task SetNodeCommandRequestAsync(NodeState request, string? nodeId = null, CancellationToken token = default)
		{
			if (nodeId is not null)
			{
				if (!_nodeStates.ContainsKey(nodeId))
				{
					throw new KeyNotFoundException($"{nameof(nodeId)}: '{nodeId}'");
				}

				if (_nodeStateRequestChannels.TryGetValue(nodeId, out var channel))
				{
					await channel.Writer.WriteAsync(request, token);
				}
				else
				{
					throw new Exception("Unexpected error, should never happen. in NodeLifetimeService");
				}
			}
			else
			{
				foreach (var node in _nodeStates.Keys)
				{
					await SetNodeCommandRequestAsync(request, node, token);

					//TODO: Not Should if this is the right place
					_ = SetNodeStateResponseAsync(node, NodeState.Preparing, CancellationToken.None);
				}
			}
		}

		public IAsyncEnumerable<NodeState> GetNodeRequestStatesAllAsync(string nodeId, CancellationToken token = default)
		{
			if (_nodeStateRequestChannels.TryGetValue(nodeId, out var channel))
			{
				return channel.Reader.ReadAllAsync(token);
			}
			throw new KeyNotFoundException($"{nameof(nodeId)}: '{nodeId}'");
		}

		public async Task SetNodeStateResponseAsync(string nodeId, NodeState nodeCommand, CancellationToken token = default)
		{
			if (_nodeStateResponseChannels.TryGetValue(nodeId, out var channel))
			{
				await channel.Writer.WriteAsync(nodeCommand, token);
			}
			else
			{
				throw new KeyNotFoundException($"{nameof(nodeId)}: '{nodeId}'");
			}
			await _allUpdatesChannel.Writer.WriteAsync((nodeId, nodeCommand), token);
		}




	}
}
