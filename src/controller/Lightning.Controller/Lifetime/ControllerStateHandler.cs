using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	internal class ControllerStateHandler :
		StateHandler<ControllerState>,
		IControllerStateNotifier,
		IControllerStateReceiver
	{
		private readonly INodeLifetimeService _nodeLifetimeService;

		public ControllerStateHandler(INodeLifetimeService nodeLifetimeService, ILogger<ControllerStateHandler>? logger = null) : base(logger)
		{
			_nodeLifetimeService = nodeLifetimeService;
		}

		protected override async Task StateChangedCallback(ControllerState response)
		{
			if (response == ControllerState.Live)
			{
				await _nodeLifetimeService.GoLiveAsync();
			}
			if (response == ControllerState.Ready)
			{
				await _nodeLifetimeService.GoReadyAsync();
			}
		}
	}
}
