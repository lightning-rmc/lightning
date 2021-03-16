using Lightning.Controller.Lifetime;
using Microsoft.AspNetCore.SignalR;

namespace Lightning.Controller.Host.Hubs
{
	public class ControllerLifetimeHub : Hub
	{
		private readonly IControllerStateNotifier _stateNotifier;
		private readonly IControllerStateReceiver _stateReceiver;

		public ControllerLifetimeHub(IControllerStateNotifier controllerStateNotifier, IControllerStateReceiver controllerStateReceiver)
		{
			_stateNotifier = controllerStateNotifier;
			_stateReceiver = controllerStateReceiver;
		}
	}
}
