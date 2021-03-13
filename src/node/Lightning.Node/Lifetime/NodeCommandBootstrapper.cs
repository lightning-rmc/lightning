using Lightning.Core.Rendering;
using Lightning.Core.Presentation;
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
		private readonly INodeCommandReceiver _receiver;
		private readonly ILogger<NodeCommandBootstrapper>? _logger;

		public NodeCommandBootstrapper(INodeCommandReceiver receiver, ILogger<NodeCommandBootstrapper>? logger = null)
		{
			_receiver = receiver;
			_logger = logger;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await _receiver.InvokeCommandRequestAsync(NodeCommandRequest.OnStart, cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await _receiver.InvokeCommandRequestAsync(NodeCommandRequest.OnShutdown, cancellationToken);
		}
	}
}
