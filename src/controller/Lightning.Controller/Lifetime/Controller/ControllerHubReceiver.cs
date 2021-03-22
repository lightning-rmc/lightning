using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime.Controller
{
	public class ControllerHubReceiver : Hub<IControllerHubClient>, IControllerHubServer
	{
		private readonly IControllerStateReceiver _controllerStateReceiver;

		public ControllerHubReceiver(IControllerStateReceiver controllerStateReceiver)
		{
			_controllerStateReceiver = controllerStateReceiver;
		}

		public async Task GoLive()
		{
			await _controllerStateReceiver.InvokeStateChangeAsync(ControllerState.Live);
		}

		public async Task GoReady()
		{
			await _controllerStateReceiver.InvokeStateChangeAsync(ControllerState.Ready);
		}
	}
}
