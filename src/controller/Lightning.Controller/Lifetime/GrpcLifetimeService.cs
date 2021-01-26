using Grpc.Core;
using Lightning.Core.Generated;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public class GrpcLifetimeService : GrpcLifeTimeService.GrpcLifeTimeServiceBase
	{
		private readonly INodeLifetimeService _lifetimeService;
		private readonly ILogger<GrpcLifetimeService>? _logger;

		public GrpcLifetimeService(INodeLifetimeService lifetimeService, ILogger<GrpcLifetimeService>? logger = null)
		{
			_lifetimeService = lifetimeService;
			_logger = logger;
		}

		public override async Task Connect(GeneralRequest request, IServerStreamWriter<NodeStateMessage> responseStream, ServerCallContext context)
		{
			//TODO: get real Id
			var id = string.Empty;
			var stream = _lifetimeService.GetNodeStatesAllAsync(id);

			await foreach (var state in stream)
			{
				//Note: No need to tell the node that it is offline
				if (state != NodeState.Offline)
				{
					await responseStream.WriteAsync(new NodeStateMessage
					{
						State = (NodeStateMessage.Types.State)state
					});
				}
			}
		}

	}
}
