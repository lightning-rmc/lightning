using Grpc.Core;
using Lightning.Controller.Utils;
using Lightning.Core.Generated;
using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public class GrpcLifetimeService : Core.Generated.GrpcLifetimeService.GrpcLifetimeServiceBase
	{
		private readonly INodeLifetimeRequestResponsePublisher _lifetimeServicePublisher;
		private readonly INodeLifetimeService _nodeLifetimeService;
		private readonly ILogger<GrpcLifetimeService>? _logger;

		public GrpcLifetimeService(INodeLifetimeRequestResponsePublisher lifetimeServicePublisher,
			INodeLifetimeService nodeLifetimeService,
			ILogger<GrpcLifetimeService>? logger = null)
		{
			_lifetimeServicePublisher = lifetimeServicePublisher;
			_nodeLifetimeService = nodeLifetimeService;
			_logger = logger;
		}

		public override Task<ConnectResponse> Connect(ConnectMessage request, ServerCallContext context)
		{
			var nodeId = context.GetHttpContext().GetNodeId();
			_nodeLifetimeService.TryRegisterNode(nodeId);

			return Task.FromResult<ConnectResponse>(new());
		}
		public override async Task NodeStateChannel(IAsyncStreamReader<NodeStateRequestMessage> requestStream, IServerStreamWriter<NodeStateResponseMessage> responseStream, ServerCallContext context)
		{
			//TODO: handle stream closing
			var nodeId = context.GetHttpContext().GetNodeId();
			_ = Task.Run(async () =>
			{
				try
				{
					await foreach (var response in requestStream.ReadAllAsync(context.CancellationToken))
					{
						_logger.LogDebug("Node {nodeId} state update: {state}", nodeId, (NodeState)response.State);
						await _lifetimeServicePublisher.SetNodeStateResponseAsync(nodeId, (NodeState)response.State);
					}
				}
				catch
				{
					_logger?.LogWarning("Node disconnected {nodeId}", nodeId);
					await _lifetimeServicePublisher.SetNodeStateResponseAsync(nodeId, NodeState.Offline);
				}
			});
			try
			{
				await foreach (var request in _lifetimeServicePublisher.GetNodeRequestStatesAllAsync(nodeId, context.CancellationToken))
				{
					var message = new NodeStateResponseMessage
					{
						State = (int)request
					};
					await responseStream.WriteAsync(message);
				}
			}
			catch { }
		}

		public override Task NodeCommandChannel(
			IAsyncStreamReader<NodeCommandResponseMessage> requestStream,
			IServerStreamWriter<NodeCommandRequestMessage> responseStream,
			ServerCallContext context)
		{
			return Task.CompletedTask;
		}

		public override async Task GetLayerActivationStream(GeneralRequest request,
			IServerStreamWriter<LayerActivationMessage> responseStream,
			ServerCallContext context)
		{
			await responseStream.WriteAsync(new()
			{
				Active = true,
				LayerId = "myLayer"
			});
			await Task.Delay(4000);

			await responseStream.WriteAsync(new()
			{
				Active = false,
				LayerId = "myLayer"
			});
			await responseStream.WriteAsync(new()
			{
				Active = true,
				LayerId = "myLayer"
			});
			await Task.Delay(20000);
		}
	}
}
