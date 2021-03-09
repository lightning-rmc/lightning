using Grpc.Core;
using Lightning.Controller.Utils;
using Lightning.Core.Generated;
using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
using System;
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

		public override async Task Connect(
			IAsyncStreamReader<NodeCommandResponseMessage> requestStream,
			IServerStreamWriter<NodeCommandRequestMessage> responseStream,
			ServerCallContext context)
		{
			//TODO: handle stream closing
			var nodeId = context.GetHttpContext().GetNodeId();
			_ = Task.Run(async () =>
			{
				await foreach (var response in requestStream.ReadAllAsync())
				{
					if ((NodeCommandResponse)response.Command == NodeCommandResponse.IsConnected)
					{
						_nodeLifetimeService.TryRegisterNode(nodeId);
					} else
					{
						await _lifetimeServicePublisher.SetNodeResponseAsync(nodeId, (NodeCommandResponse)response.Command);
					}
				}
			});

			await foreach (var request in _lifetimeServicePublisher.GetNodeRequestsAllAsync(nodeId))
			{
				var message = new NodeCommandRequestMessage
				{
					Command = (int)request
				};
				await responseStream.WriteAsync(message);
			}
		}

		public override async Task GelLayerActivationStream(GeneralRequest request,
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
