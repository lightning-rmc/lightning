using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public class ControllerCommandBootstrapper : IHostedService
	{
		private readonly IControllerCommandReceiver _commandReceiver;

		public ControllerCommandBootstrapper(IControllerCommandReceiver commandReceiver)
		{
			_commandReceiver = commandReceiver;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await _commandReceiver.InvokeCommandRequestAsync(ControllerCommandRequest.OnStart, cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await _commandReceiver.InvokeCommandRequestAsync(ControllerCommandRequest.OnShutdown, cancellationToken);
		}
	}
}
