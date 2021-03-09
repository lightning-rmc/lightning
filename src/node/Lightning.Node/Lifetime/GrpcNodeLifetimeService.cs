using Grpc.Core;
using Lightning.Core.Generated;
using Lightning.Core.Lifetime;
using Lightning.Core.Rendering;
using Lightning.Core.Utils;
using Lightning.Node.Communications;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Node.Lifetime
{
	internal class GrpcNodeLifetimeService : ICreateOnStartup
	{
		private readonly INodeStateManager _stateManager;
		private GrpcLifetimeService.GrpcLifetimeServiceClient _grpcClient = null!;

		public GrpcNodeLifetimeService(IConnectionManager connectionManager,
									   IHostApplicationLifetime hostLifetime,
									   INodeStateManager stateManager)
		{
			_stateManager = stateManager;

			//TODO: Maye check if application is already started.
			hostLifetime.ApplicationStarted.Register(() =>
			{
				_grpcClient = connectionManager.GetLifetimeServiceClient();

				Task.Run(GetLifeTimeUpdatesAsync);
			});
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
				HandleRequest(command);
			}
			//TODO: Handle Reconnect
		}

		private async IAsyncEnumerable<NodeCommandResponseMessage> HandleResponseAllAsync()
		{
			await foreach (var response in _stateManager.GetNodeCommandResponseAllAsync())
			{
				var message = new NodeCommandResponseMessage
				{
					Command = (int)response
				};
				yield return message;
			}

		}

		private  void HandleRequest(NodeCommandRequestMessage message)
		{
			//TODO: Handle it more Secure...
			var command = (NodeCommandRequest)message.Command;

			//TODO: Check Conditions if it can be set?
			//TODO: Set to internal State...like ready, live, etc..
			_stateManager.ReportNodeCommandRequestAsync(command);
		}
		#endregion

	}
}
