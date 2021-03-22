using Lightning.Controller.Lifetime;
using Lightning.Controller.Lifetime.Controller;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime.Layers
{
	public class LiveHubReceiver : Hub<ILiveHubClient> , ILiveHubServer
	{
		private readonly ILayerActivationService _activationService;
		private readonly IControllerStateReceiver _controllerStateReceiver;

		public LiveHubReceiver(ILayerActivationService activationService, IControllerStateReceiver controllerStateReceiver)
		{
			_activationService = activationService;
			_controllerStateReceiver = controllerStateReceiver;
		}

		public async override Task OnConnectedAsync()
		{
			var activationStates = _activationService.GetLayerActivationStates();
			foreach (var (LayerId, IsActive) in activationStates)
			{
				await Clients.Caller.LayerActivationChangedAsync(LayerId, IsActive);
			}
		}


		public async Task SetLayerActivation(string layerId, bool active)
		{
			await _activationService.SetLayerActivationAsync(layerId, active);
		}
	}
}
