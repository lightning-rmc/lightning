using Grpc.Core;
using Lightning.Core.Generated;
using Lightning.Core.Lifetime;
using Lightning.Core.Utils;
using Lightning.Node.Communications;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lightning.Node.Lifetime
{
	internal class GrpcNodeLifetimeService : ICreateOnStartup
	{
		private readonly INodeLifetimeReceiver _lifetimeReceiver;
		private GrpcLifetimeService.GrpcLifetimeServiceClient _grpcClient = null!;

		public GrpcNodeLifetimeService(IConnectionManager connectionManager,
									   INodeLifetimeNotifier nodeliftime,
									   INodeLifetimeReceiver lifetimeReceiver)
		{
			_lifetimeReceiver = lifetimeReceiver;

			//TODO: Maye check if application is already started.
			nodeliftime.CommandResponded += (s, e) =>
			{
				if (e.Response == NodeCommandResponse.IsConnected)
				{
					_grpcClient = connectionManager.GetLifetimeServiceClient();
					Task.Run(GetLifeTimeUpdatesAsync);
				}
			};
		}

		#region HandleGrpc Stream

		private async Task GetLifeTimeUpdatesAsync()
		{
			var result = _grpcClient.NodeCommandChannel(new());
			_ = Task.Run(async () =>
			{
				await foreach (var message in HandleResponseAllAsync())
				{
					await result.RequestStream.WriteAsync(message);
				}
			});

			await foreach (var command in result.ResponseStream.ReadAllAsync())
			{
				//TODO: not sure if we should await the result...
				_ = HandleRequestAsync(command);
			}
			//TODO: Handle Reconnect
		}

		private async IAsyncEnumerable<NodeCommandResponseMessage> HandleResponseAllAsync()
		{
			await foreach (var response in _lifetimeReceiver.GetNodeCommandResponsesAllAsync())
			{
				var message = new NodeCommandResponseMessage
				{
					Command = (int)response
				};
				yield return message;
			}

		}

		private async Task HandleRequestAsync(NodeCommandRequestMessage message)
		{
			//TODO: Handle it more Secure...
			var command = (NodeCommandRequest)message.Command;
			await _lifetimeReceiver.InvokeCommandRequestAsync(command);
		}
		#endregion

	}
}
