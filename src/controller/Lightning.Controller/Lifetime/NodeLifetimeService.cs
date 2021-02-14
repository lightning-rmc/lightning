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
				await Task.Delay(3000);
				await this.GoLiveAsync();
				await Task.Delay(2000);
				await this.GoReadyAsync();
				await this.GoLiveAsync();
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
			//TODO: Not sure if needed?
			//Note: Remove also from ProjectManager
			return true;
		}

		public bool TryRegisterNode(string nodeId)
		{
			if (_nodeStates.ContainsKey(nodeId))
			{
				return false;
			}
			_nodeStates.Add(nodeId, NodeState.Offline);
			_nodeCommandRequestsChannels.Add(nodeId, Channel.CreateUnbounded<NodeCommandRequest>());
			_nodeCommandResponsesChannels.Add(nodeId, Channel.CreateUnbounded<NodeCommandResponse>());
			_logger?.LogInformation("Register new Node with id:'{nodeId}'.", nodeId);
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
