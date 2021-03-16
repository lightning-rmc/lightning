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
		private readonly INodeStateReceiver _NodeCommandReceiver;
		private GrpcLifetimeService.GrpcLifetimeServiceClient _grpcClient = null!;

		public GrpcNodeLifetimeService(IConnectionManager connectionManager,
									   INodeStateNotifier nodeCommandNotifier,
									   INodeStateReceiver nodeCommandReceiver)
		{
			_NodeCommandReceiver = nodeCommandReceiver;

			//TODO: Maye check if application is already started.
			nodeCommandNotifier.StateChanged += (s, e) =>
			{
				if (e.Response == NodeState.Connected)
				{
					_grpcClient = connectionManager.GetLifetimeServiceClient();
					Task.Run(GetLifeTimeUpdatesAsync);
				}
			};
		}

		#region HandleGrpc Stream

		private async Task GetLifeTimeUpdatesAsync()
		{
			var result = _grpcClient.NodeStateChannel(new());
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

		private async IAsyncEnumerable<NodeStateRequestMessage> HandleResponseAllAsync()
		{
			await foreach (var response in _NodeCommandReceiver.GetStateResponsesAllAsync())
			{
				var message = new NodeStateRequestMessage
				{
					State = (int)response
				};
				yield return message;
			}

		}

		private async Task HandleRequestAsync(NodeStateResponseMessage message)
		{
			//TODO: Handle it more Secure...
			var command = (NodeState)message.State;
			await _NodeCommandReceiver.InvokeStateChangeAsync(command);
		}
		#endregion

	}
}
