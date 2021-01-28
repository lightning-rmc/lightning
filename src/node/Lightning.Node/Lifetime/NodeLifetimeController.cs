using Grpc.Core;
using Lightning.Core.Generated;
using Lightning.Core.Lifetime;
using Lightning.Core.Rendering;
using Lightning.Core.Utils;
using Lightning.Node.Communications;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Node.Lifetime
{
	public class NodeLifetimeController : ICreateOnStartup
	{
		private readonly IRenderHost _renderHost;
		private GrpcLifeTimeService.GrpcLifeTimeServiceClient _grpcClient = null!;

		public NodeLifetimeController(IGrpcConnectionManager connectionManager,IRenderHost renderHost,  IHostApplicationLifetime hostLifetime)
		{

			hostLifetime.ApplicationStarted.Register(() =>
			{
				_grpcClient = connectionManager.GetLifetimeServiceClient();

				Task.Factory.StartNew(GetLifeTimeUpdatesAsync, TaskCreationOptions.LongRunning);
			});
			_renderHost = renderHost;
		}

		private async Task GetLifeTimeUpdatesAsync()
		{
			var result =  _grpcClient.Connect(new());
			await foreach (var updateSate in result.ResponseStream.ReadAllAsync())
			{
				switch (updateSate.State)
				{
					case NodeStateMessage.Types.State.Info:
						ShowInfoMessage();
						break;
					case NodeStateMessage.Types.State.Edit:
						ShowBlankPage();
						break;
					case NodeStateMessage.Types.State.Live:
						GoLive();
						break;
					case NodeStateMessage.Types.State.Error:
						break;
					default:
						break;
				}
			}

			//TODO: Handle Reconnect
		}

		private void GoLive()
		{
			throw new NotImplementedException();
		}

		private void ShowBlankPage()
		{
			throw new NotImplementedException();
		}

		private void ShowInfoMessage()
		{
			throw new NotImplementedException();
		}
	}
}
