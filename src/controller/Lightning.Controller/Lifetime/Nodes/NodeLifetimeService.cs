using Lightning.Controller.Projects;
using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime.Nodes
{
	internal class NodeLifetimeService : INodeLifetimeService, INodeLifetimeRequestResponsePublisher
	{
		private readonly ILogger<NodeLifetimeService>? _logger;
		private readonly Dictionary<string, Channel<NodeState>> _nodeStateRequestChannels;
		private readonly ConcurrentDictionary<string, NodeState> _nodeStates;
		private readonly ConcurrentDictionary<Channel<NodeStateUpdate>, object?> _allUpdatesChannels;
		private readonly List<Channel<string>> _nodeConnectedUpdatesChannels;
		private readonly IProjectManager _projectManager;


		public NodeLifetimeService(IProjectManager projectManager, ILogger<NodeLifetimeService>? logger = null)
		{
			_projectManager = projectManager;
			_nodeStateRequestChannels = new Dictionary<string, Channel<NodeState>>();
			_allUpdatesChannels = new ConcurrentDictionary<Channel<NodeStateUpdate>, object?>();
			_nodeStates = new ConcurrentDictionary<string, NodeState>();
			_nodeConnectedUpdatesChannels = new List<Channel<string>>();
			_logger = logger;

			_projectManager.ProjectLoaded += async (s, e) =>
			{
				ClearAllNodes();
				var nodes = _projectManager.GetNodes();
				foreach (var node in nodes)
				{
					await TryRegisterNodeAsync(node.Id);
				}
			};
		}

		public async IAsyncEnumerable<NodeStateUpdate> GetAllNodeStatesAllAsync([EnumeratorCancellation] CancellationToken token = default)
		{
			var channel = Channel.CreateUnbounded<NodeStateUpdate>();
			_allUpdatesChannels.TryAdd(channel, null);
			try
			{
				await foreach (var state in channel.Reader.ReadAllAsync(token))
				{
					yield return state;
				}
			}
			finally
			{
				channel.Writer.Complete();
				_allUpdatesChannels.TryRemove(channel, out _);
			}
		}


		public async IAsyncEnumerable<string> GetAllNodeConnectedUpdatesAllAsync([EnumeratorCancellation] CancellationToken token = default)
		{
			var channel = Channel.CreateUnbounded<string>();
			_nodeConnectedUpdatesChannels.Add(channel);
			try
			{
				await foreach (var nodeId in channel.Reader.ReadAllAsync(token))
				{
					yield return nodeId;
				}
			}
			finally
			{
				channel.Writer.Complete();
				_nodeConnectedUpdatesChannels.Remove(channel);
			}
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
			_nodeStates.Remove(nodeId, out _);
			return true;
		}


		private void ClearAllNodes()
		{
			foreach (var item in _nodeStateRequestChannels)
			{
				item.Value.Writer.TryComplete();
			}
			_nodeStateRequestChannels.Clear();
			_nodeStates.Clear();
		}


		public async Task TryRegisterNodeAsync(string nodeId)
		{
			if (!_projectManager.GetNodes().Where(n => n.Id == nodeId).Any())
			{
				foreach (var channel in _nodeConnectedUpdatesChannels)
				{
					await channel.Writer.WriteAsync(nodeId);
				}
			}
			if (!_nodeStates.ContainsKey(nodeId))
			{
				_nodeStates.AddOrUpdate(nodeId, NodeState.Offline, (key, old) => NodeState.Offline);
				_nodeStateRequestChannels.Add(nodeId, Channel.CreateUnbounded<NodeState>());
				_logger?.LogInformation("Register new Node with id:'{nodeId}'.", nodeId);
				var result = _projectManager.TryAddNode(nodeId);
				//TODO: add logging
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
			var list = _allUpdatesChannels.Keys.ToArray();
			foreach (var channel in list)
			{
				try
				{
					await channel.Writer.WriteAsync(new NodeStateUpdate(nodeId, state), token);
				}
				catch (Exception e)
				{
					//TODO: add logging
					_logger?.LogWarning(e, "SetNodeStateResponseAsync");
				}
			}
			_nodeStates.AddOrUpdate(nodeId, state, (key, old) => state);
		}
	}
}
