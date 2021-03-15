using Lightning.Controller.Projects;
using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	internal class NodeLifetimeService : INodeLifetimeService, INodeLifetimeRequestResponsePublisher
	{
		private readonly ILogger<NodeLifetimeService>? _logger;
		//TODO: maybe change to ConcurrentDictionary
		private readonly Dictionary<string, Channel<NodeCommandResponse>> _nodeCommandResponsesChannels;
		private readonly Dictionary<string, Channel<NodeCommandRequest>> _nodeCommandRequestsChannels;
		private readonly Dictionary<string, NodeState> _nodeStates;
		private readonly Channel<(string nodeId, NodeCommandResponse state)> _allUpdatesChannel;
		private readonly IProjectManager _projectManager;


		public NodeLifetimeService(IProjectManager projectManager, ILogger<NodeLifetimeService>? logger = null)
		{
			_projectManager = projectManager;
			_nodeCommandResponsesChannels = new Dictionary<string, Channel<NodeCommandResponse>>();
			_nodeCommandRequestsChannels = new Dictionary<string, Channel<NodeCommandRequest>>();
			_allUpdatesChannel = Channel.CreateUnbounded<(string, NodeCommandResponse)>();
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

		public IAsyncEnumerable<(string NodeId, NodeCommandResponse Command)> GetAllNodeCommandsAllAsync()
			=> _allUpdatesChannel.Reader.ReadAllAsync();

		public IEnumerable<(string NodeId, NodeState State)> GetAllNodeStates()
			=> _nodeStates.Select(kv => (kv.Key, kv.Value));

		public IAsyncEnumerable<NodeCommandResponse> GetNodeCommandsAllAsync(string nodeId)
		{
			if (_nodeCommandResponsesChannels.TryGetValue(nodeId, out var channel))
			{
				return channel.Reader.ReadAllAsync();
			}
			throw new KeyNotFoundException($"{nameof(nodeId)}: '{nodeId}'");
		}

		public bool TryRemoveNode(string nodeId)
		{
			if (_nodeCommandRequestsChannels.TryGetValue(nodeId, out var requestsChannel))
			{
				requestsChannel.Writer.TryComplete();
				_nodeCommandRequestsChannels.Remove(nodeId);
			}
			if (_nodeCommandResponsesChannels.TryGetValue(nodeId, out var responseChannel))
			{
				responseChannel.Writer.TryComplete();
				_nodeCommandResponsesChannels.Remove(nodeId);
			}
			_nodeStates.Remove(nodeId);
			return true;
		}

		private void ClearAllNodes()
		{
			foreach (var item in _nodeCommandRequestsChannels)
			{
				item.Value.Writer.TryComplete();
			}
			foreach (var item in _nodeCommandResponsesChannels)
			{
				item.Value.Writer.TryComplete();
			}
			_nodeCommandRequestsChannels.Clear();
			_nodeCommandResponsesChannels.Clear();
			_nodeStates.Clear();
		}

		public void TryRegisterNode(string nodeId)
		{
			if (!_nodeStates.ContainsKey(nodeId))
			{
				_nodeStates.Add(nodeId, NodeState.Offline);
				_nodeCommandRequestsChannels.Add(nodeId, Channel.CreateUnbounded<NodeCommandRequest>());
				_nodeCommandResponsesChannels.Add(nodeId, Channel.CreateUnbounded<NodeCommandResponse>());
				_logger?.LogInformation("Register new node for updates with id:'{nodeId}'.", nodeId);
			}
		}

		public async Task SetNodeCommandRequestAsync(NodeCommandRequest request, string? nodeId = null)
		{
			if (nodeId is not null)
			{
				if (!_nodeStates.ContainsKey(nodeId))
				{
					throw new KeyNotFoundException($"{nameof(nodeId)}: '{nodeId}'");
				}

				if (_nodeCommandRequestsChannels.TryGetValue(nodeId, out var channel))
				{
					await channel.Writer.WriteAsync(request);
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
					await SetNodeCommandRequestAsync(request, node);

					//TODO: Not Should if this is the right place
					_ = SetNodeResponseAsync(node, NodeCommandResponse.IsPreparing);
				}
			}
		}

		public IAsyncEnumerable<NodeCommandRequest> GetNodeRequestsAllAsync(string nodeId)
		{
			if (_nodeCommandRequestsChannels.TryGetValue(nodeId, out var channel))
			{
				return channel.Reader.ReadAllAsync();
			}
			throw new KeyNotFoundException($"{nameof(nodeId)}: '{nodeId}'");
		}

		public async Task SetNodeResponseAsync(string nodeId, NodeCommandResponse nodeCommand)
		{
			if (_nodeCommandResponsesChannels.TryGetValue(nodeId, out var channel))
			{
				await channel.Writer.WriteAsync(nodeCommand);
			}
			else
			{
				throw new KeyNotFoundException($"{nameof(nodeId)}: '{nodeId}'");
			}
			await _allUpdatesChannel.Writer.WriteAsync((nodeId, nodeCommand));
		}
	}
}
