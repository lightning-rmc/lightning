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
	internal class GrpcNodeLifetimeService : INodeLifetimeService, ICreateOnStartup
	{
		private readonly IRenderHost _renderHost;
		private readonly Channel<NodeCommandResponse> _responseChannel;
		private readonly Channel<NodeCommandRequest> _requestChannel;
		private GrpcLifetimeService.GrpcLifetimeServiceClient _grpcClient = null!;

		public GrpcNodeLifetimeService(IConnectionManager connectionManager, IRenderHost renderHost, IHostApplicationLifetime hostLifetime)
		{
			//TODO: Maye check if application is already started.
			hostLifetime.ApplicationStarted.Register(() =>
			{
				_grpcClient = connectionManager.GetLifetimeServiceClient();

				Task.Run(GetLifeTimeUpdatesAsync);
			});
			_renderHost = renderHost;
			_requestChannel = Channel.CreateUnbounded<NodeCommandRequest>();
			_responseChannel = Channel.CreateUnbounded<NodeCommandResponse>();			
		}

		public async Task SetCommandResponseAsync(NodeCommandResponse command)
			=> await _responseChannel.Writer.WriteAsync(command);

		public IAsyncEnumerable<NodeCommandRequest> GetCommandRequestsAllAsync()
			=> _requestChannel.Reader.ReadAllAsync();



		#region HandleGrpc Stream

		private async Task GetLifeTimeUpdatesAsync()
		{
			var result = _grpcClient.Connect(new());
			_ = Task.Run(async () =>
			{
				await foreach (var message in HandleResponseAllAsync())
				{
					await result.RequestStream.WriteAsync(message);
				}
			});

			await foreach (var command in result.ResponseStream.ReadAllAsync())
			{
				await HandleRequestAsync(command);

			}
			//TODO: Handle Reconnect
		}

		private async IAsyncEnumerable<NodeCommandResponseMessage> HandleResponseAllAsync()
		{
			await foreach (var response in _responseChannel.Reader.ReadAllAsync())
			{
				//TODO: handle Conditions

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

			//TODO: Check Conditions if it can be set?
			//TODO: Set to internal State...like ready, live, etc..

			switch (command)
			{
				case NodeCommandRequest.GoLive:
					await _renderHost.StartAsync();
					break;
				case NodeCommandRequest.GoReady:
					_renderHost.Stop();
					break;
				case NodeCommandRequest.ShowInfo:
					break;
				case NodeCommandRequest.HideInfo:
					break;
				default:
					break;
			}

			await _requestChannel.Writer.WriteAsync(command);
		} 
		#endregion

	}
}
