using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime.Controller
{
	public class ControllerStateBootstrapper : IHostedService
	{
		private readonly IControllerStateReceiver _commandReceiver;

		public ControllerStateBootstrapper(IControllerStateReceiver commandReceiver)
		{
			_commandReceiver = commandReceiver;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
			=> await _commandReceiver.InvokeStateChangeAsync(ControllerState.Start, cancellationToken);

		public async Task StopAsync(CancellationToken cancellationToken)
			=> await _commandReceiver.InvokeStateChangeAsync(ControllerState.Shutdown, cancellationToken);
	}
}
