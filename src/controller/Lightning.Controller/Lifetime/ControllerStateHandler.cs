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
		public ControllerStateHandler(ILogger<ControllerStateHandler>? logger = null) : base(logger)
		{

		}

		protected override Task InvokeSubsequentStateRequest(ControllerState response)
		{
			return Task.CompletedTask;
		}
	}
}
