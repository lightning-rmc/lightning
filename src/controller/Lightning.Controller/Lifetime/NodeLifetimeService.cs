using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	internal class NodeLifetimeService : INodeLifetimeService, INodeLifetimeRequestPublisher
	{
		private readonly ILogger<NodeLifetimeService>? _logger;
		private readonly Dictionary<string, Channel<NodeCommandResponse>> _nodeCommandResponsesChannels;
		private readonly Dictionary<string, Channel<NodeCommandRequest>> _nodeCommandRequestsChannels;
		private readonly Dictionary<string, NodeState> _nodeStates;
		private readonly Channel<(string nodeId, NodeCommandResponse state)> _allUpdatesChannel;


		public NodeLifetimeService(ILogger<NodeLifetimeService>? logger = null)
		{
			_nodeCommandResponsesChannels = new Dictionary<string, Channel<NodeCommandResponse>>();
			_nodeCommandRequestsChannels = new Dictionary<string, Channel<NodeCommandRequest>>();
			_allUpdatesChannel = Channel.CreateUnbounded<(string, NodeCommandResponse)>();
			_nodeStates = new Dictionary<string, NodeState>();
			_logger = logger;
			//TODO: Remove Test Register


			//TODO: Remove testcase
			Task.Run(async () =>
			{
				//var rnd = new Random();
				//while (true)
				//{
				//	await UpdateNodeStateAsync(rnd.NextDouble() > 0.5 ? NodeState.Live : NodeState.Error);
				//	await Task.Delay(2000);
				//}

				await Task.Delay(5000);
			});
		}

		public IAsyncEnumerable<(string NodeId, NodeCommandResponse Command)> GetAllNodeCommandsAllAsync()
			=> _allUpdatesChannel.Reader.ReadAllAsync();

		public IEnumerable<(string NodeId, NodeState Satte)> GetAllNodeStates()
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
			return true;
		}

		public bool TryRegisterNode(string nodeId)
		{
			return true;
		}

		public async Task SetNodeCommandRequestsAsync(NodeCommandRequest request, string? nodeId = null)
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
					await SetNodeCommandRequestsAsync(request, node);
				}
			}
		}

		public IAsyncEnumerable<NodeCommandRequest> GetNodeRequestsAllAsync(string nodeId)
		{
			throw new NotImplementedException();
		}

		public Task SetNodeResponseAsync(string nodeId, NodeCommandResponse nodeCommand)
		{
			throw new NotImplementedException();
		}
















		//public bool TryRegisterNode(string nodeId)
		//{
		//	if (_nodes.ContainsKey(nodeId))
		//	{
		//		return false;
		//	}
		//	_nodes.Add(nodeId, NodeState.Offline);
		//	_nodeChannels.Add(nodeId, Channel.CreateUnbounded<NodeState>());
		//	_logger?.LogInformation("Register new Node with id:'{nodeId}'.", nodeId);
		//	return true;
		//}





		//public async Task SetNodeCommandRequestsAsync(NodeState state, string? nodeId = null)
		//{
		//	if (nodeId is not null)
		//	{
		//		if (!_nodes.ContainsKey(nodeId))
		//		{
		//			throw new KeyNotFoundException($"{nameof(nodeId)}: '{nodeId}'");
		//		}

		//		_nodes[nodeId] = state;
		//		await _nodeChannels[nodeId].Writer.WriteAsync(state);
		//		await _allUpdatesChannel.Writer.WriteAsync((nodeId, state));
		//		_logger?.LogDebug("NodeState from node '{nodeId}' changed to '{state}'", nodeId, state);
		//	}
		//	else
		//	{
		//		foreach (var node in _nodes.Keys)
		//		{
		//			await SetNodeCommandRequestsAsync(state, node);
		//		}
		//	}
		//}

		//public IEnumerable<(string NodeId, NodeState State)> GetAllNodeStates()
		//	=> _nodes.Select(ns => (ns.Key, ns.Value));

		//public bool RemoveNode(string nodeId)
		//{
		//	//TODO: Not sure if needed?
		//	//Note: Remove also from ProjectManager
		//	throw new System.NotImplementedException();
		//}

		//public IAsyncEnumerable<NodeCommandResponse> GetNodeCommandsAllAsync(string nodeId)
		//{
		//	throw new NotImplementedException();
		//}

		//public IAsyncEnumerable<(string NodeId, NodeCommandResponse Command)> GetAllNodeCommandsAllAsync()
		//{
		//	throw new NotImplementedException();
		//}

		//public Task SetNodeCommandRequestsAsync(NodeCommandRequest request, string? nodeId = null)
		//{
		//	throw new NotImplementedException();
		//}
	}
}
