using Lightning.Controller.Projects;
using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
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
		private readonly ConcurrentDictionary<string, NodeState> _nodeStates;
		private readonly Channel<NodeStateUpdate> _allUpdatesChannel;
		private readonly IProjectManager _projectManager;


		public NodeLifetimeService(IProjectManager projectManager, ILogger<NodeLifetimeService>? logger = null)
		{
			_projectManager = projectManager;
			_nodeStateResponseChannels = new Dictionary<string, Channel<NodeState>>();
			_nodeStateRequestChannels = new Dictionary<string, Channel<NodeState>>();
			_allUpdatesChannel = Channel.CreateUnbounded<NodeStateUpdate>(new() { SingleReader = false, SingleWriter = false });
			_nodeStates = new ConcurrentDictionary<string, NodeState>();
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


		public async IAsyncEnumerable<NodeStateUpdate> GetAllNodeStatesAllAsync(CancellationToken token = default)
		{
			try
			{
				await foreach (var update in _allUpdatesChannel.Reader.ReadAllAsync())
				{
					yield return update;
				}

			}
			finally
			{
				// Remove custom channel here
			}
		}


		public IEnumerable<(string NodeId, NodeState State)> GetAllNodeStates()
			=> _nodeStates.Select(kv => (kv.Key, kv.Value));


		public IAsyncEnumerable<NodeState> GetNodeStatsAllAsync(string nodeId, CancellationToken token = default)
		{
			if (_nodeStateResponseChannels.TryGetValue(nodeId, out var channel))
			{
				return channel.Reader.ReadAllAsync(token);
			}
			throw new KeyNotFoundException($"{nameof(nodeId)}: '{nodeId}'");
		}


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
			_nodeStates.Remove(nodeId, out _);
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
				_nodeStates.AddOrUpdate(nodeId, NodeState.Offline, (key, old) => NodeState.Offline);
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
					//_ = SetNodeStateResponseAsync(node, NodeState.Preparing, CancellationToken.None);
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


		public async Task SetNodeStateResponseAsync(string nodeId, NodeState state, CancellationToken token = default)
		{
			_logger?.LogInformation("Node {nodeId} state update: {state}", nodeId, state);
			await _allUpdatesChannel.Writer.WriteAsync(new(nodeId, state));

			_nodeStates.AddOrUpdate(nodeId, state, (key, old) => state);
		}
	}
}
