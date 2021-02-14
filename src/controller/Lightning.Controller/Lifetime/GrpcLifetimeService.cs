using Grpc.Core;
using Lightning.Core.Generated;
using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
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


			//TODO: get real Id
			var id = "test";
			_ = Task.Factory.StartNew(async () =>
			{
				await foreach (var response in requestStream.ReadAllAsync())
				{
					//TODO: handle for secure check.
					await _lifetimeServicePublisher.SetNodeResponseAsync(id, (NodeCommandResponse)response.Commnad);
				}

			}, TaskCreationOptions.LongRunning);


			await foreach (var request in _lifetimeServicePublisher.GetNodeRequestsAllAsync(id))
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
