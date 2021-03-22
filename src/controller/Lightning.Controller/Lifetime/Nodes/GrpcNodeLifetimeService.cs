using Grpc.Core;
using Lightning.Controller.Utils;
using Lightning.Core.Generated;
using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime.Nodes
{
	public class GrpcNodeLifetimeService : Core.Generated.GrpcLifetimeService.GrpcLifetimeServiceBase
	{
		private readonly INodeLifetimeRequestResponsePublisher _lifetimeServicePublisher;
		private readonly INodeLifetimeService _nodeLifetimeService;
		private readonly ILayerActivationService _layerActivationService;
		private readonly ILogger<GrpcNodeLifetimeService>? _logger;

		public GrpcNodeLifetimeService(INodeLifetimeRequestResponsePublisher lifetimeServicePublisher,
			INodeLifetimeService nodeLifetimeService,
			ILayerActivationService layerActivationService,
			ILogger<GrpcNodeLifetimeService>? logger = null)
		{
			_lifetimeServicePublisher = lifetimeServicePublisher;
			_nodeLifetimeService = nodeLifetimeService;
			_layerActivationService = layerActivationService;
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
			//TODO: it will send the message anyway,
			//		better it should send the message only if the node has the related layer
			try
			{
				await foreach (var layer in _layerActivationService.GetLayerActivationsAllAsync(context.CancellationToken))
				{
					await responseStream.WriteAsync(new LayerActivationMessage { Active = layer.IsActive, LayerId = layer.LayerId });
				}
			}
			catch 
			{
			}
		}
	}
}
