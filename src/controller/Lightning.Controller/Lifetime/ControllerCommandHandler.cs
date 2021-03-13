using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	internal class ControllerCommandHandler :
		CommandHandler<ControllerCommandRequest, ControllerCommandResponse>,
		IControllerCommandNotifier,
		IControllerCommandReceiver
	{
		public ControllerCommandHandler(ILogger<ControllerCommandHandler>? logger = null) : base(logger)
		{

		}

		protected override ControllerCommandResponse ConvertRequestToResponse(ControllerCommandRequest request)
		{
			return request switch
			{
				ControllerCommandRequest.OnStart => ControllerCommandResponse.OnStarted,
				ControllerCommandRequest.OnShutdown => ControllerCommandResponse.OnShutdown,
				_ => throw new InvalidOperationException($"command {request} has no related response!")
			};
		}

		protected override Task InvokeSubsequentRequest(ControllerCommandResponse response)
		{
			return Task.CompletedTask;
		}
	}
}
