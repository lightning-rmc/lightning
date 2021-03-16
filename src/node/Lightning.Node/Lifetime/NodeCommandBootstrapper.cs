using Lightning.Core.Rendering;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Lightning.Node.Communications;
using Lightning.Core.Lifetime;

namespace Lightning.Node.Lifetime
{
	internal class NodeCommandBootstrapper : IHostedService
	{
		private readonly INodeStateReceiver _receiver;
		private readonly ILogger<NodeCommandBootstrapper>? _logger;

		public NodeCommandBootstrapper(INodeStateReceiver receiver, ILogger<NodeCommandBootstrapper>? logger = null)
		{
			_receiver = receiver;
			_logger = logger;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await _receiver.InvokeStateChangeAsync(NodeState.Start, cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await _receiver.InvokeStateChangeAsync(NodeState.Shutdown, cancellationToken);
		}
	}
}
