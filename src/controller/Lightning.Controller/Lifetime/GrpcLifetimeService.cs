using Grpc.Core;
using Lightning.Controller.Utils;
using Lightning.Core.Generated;
using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public class GrpcLifetimeService : GrpcLifeTimeService.GrpcLifeTimeServiceBase
	{
		private readonly INodeLifetimeRequestResponsePublisher _lifetimeServicePublisher;
		private readonly ILogger<GrpcLifetimeService>? _logger;

		public GrpcLifetimeService(INodeLifetimeRequestResponsePublisher lifetimeServicePublisher,
			ILogger<GrpcLifetimeService>? logger = null)
		{
			_lifetimeServicePublisher = lifetimeServicePublisher;
			_logger = logger;
		}

		public override async Task Connect(
			IAsyncStreamReader<NodeCommandResponseMessage> requestStream,
			IServerStreamWriter<NodeCommandRequestMessage> responseStream,
			ServerCallContext context)
		{
			//TODO: handle stream closing
			var nodeeId = context.GetHttpContext().GetNodeId();
			if (nodeeId is null)
			{
				//TODO: handle more secure
				throw new ArgumentException();
			}
			_ = Task.Run(async () =>
			{
				await foreach (var response in requestStream.ReadAllAsync())
				{
					//TODO: handle for secure check.
					await _lifetimeServicePublisher.SetNodeResponseAsync(nodeeId, (NodeCommandResponse)response.Commnad);
				}
			});


			await foreach (var request in _lifetimeServicePublisher.GetNodeRequestsAllAsync(nodeeId))
			{
				var message = new NodeCommandRequestMessage
				{
					//TODO: Handle more Secure..
					Command = (NodeCommandRequestMessage.Types.CommandRequest)request
				};
				await responseStream.WriteAsync(message);
			}
		}
	}
}
