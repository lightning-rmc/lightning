using Grpc.Core;
using Lightning.Controller.Utils;
using Lightning.Core.Generated;
using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public class GrpcLifetimeService : Core.Generated.GrpcLifetimeService.GrpcLifetimeServiceBase
	{
		private readonly INodeLifetimeRequestResponsePublisher _lifetimeServicePublisher;
		private readonly INodeLifetimeService _nodeLifetimeService;
		private readonly ILayerActivationService _layerActivationService;
		private readonly ILogger<GrpcLifetimeService>? _logger;

		public GrpcLifetimeService(INodeLifetimeRequestResponsePublisher lifetimeServicePublisher,
			INodeLifetimeService nodeLifetimeService,
			ILayerActivationService layerActivationService,
			ILogger<GrpcLifetimeService>? logger = null)
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
				await foreach (var response in requestStream.ReadAllAsync())
				{
					await _lifetimeServicePublisher.SetNodeStateResponseAsync(nodeId, (NodeState)response.State);
				}
			});

			await foreach (var request in _lifetimeServicePublisher.GetNodeRequestStatesAllAsync(nodeId))
			{
				var message = new NodeStateResponseMessage
				{
					State = (int)request
				};
				await responseStream.WriteAsync(message);
			}
		}

		public override Task NodeCommandChannel(
			IAsyncStreamReader<NodeCommandResponseMessage> requestStream,
			IServerStreamWriter<NodeCommandRequestMessage> responseStream,
			ServerCallContext context)
		{
			return Task.CompletedTask;
		}

		public override async Task GelLayerActivationStream(GeneralRequest request,
			IServerStreamWriter<LayerActivationMessage> responseStream,
			ServerCallContext context)
		{
			//TODO: it will send the message anyway,
			//		better it should send the message only if the node has the related layer
			await foreach (var layer in _layerActivationService.GetLayerActivationsAllAsync())
			{
				await responseStream.WriteAsync(new LayerActivationMessage { Active = layer.Active, LayerId = layer.LayerId });
			}
		}
	}
}
