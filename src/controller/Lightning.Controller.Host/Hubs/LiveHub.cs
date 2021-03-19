using Lightning.Controller.Lifetime;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.Hubs
{
	public class LiveHub : Hub<ILiveHubClient>
	{
		private readonly ILayerActivationService _activationService;
		private readonly IControllerStateReceiver _controllerStateReceiver;

		public LiveHub(ILayerActivationService activationService, IControllerStateReceiver controllerStateReceiver)
		{
			_activationService = activationService;
			_controllerStateReceiver = controllerStateReceiver;
		}

		public async override Task OnConnectedAsync()
		{
			var activationStates = _activationService.GetLayerActivationStates();
			foreach (var state in activationStates)
			{
				await SetLayerActivation(state.LayerId, state.IsActive);
			}
		}


		public async Task SetLayerActivation(string layerId, bool isActive)
		{
			await _activationService.SetLayerActivationAsync(layerId, isActive);
			await Clients.Others.LayerActivationChanged(layerId, isActive);
		}

		public async Task ActivateLive()
		{
			await _controllerStateReceiver.InvokeStateChangeAsync(ControllerState.Live);
		}

		public async Task DeactivateLive()
		{
			await _controllerStateReceiver.InvokeStateChangeAsync(ControllerState.Ready);
		}
	}
}
