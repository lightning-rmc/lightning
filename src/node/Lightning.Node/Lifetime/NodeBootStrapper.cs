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
	internal class NodeBootStrapper : IHostedService
	{
		private readonly INodeLifetimeReceiver _receiver;
		private readonly ILogger<NodeBootStrapper>? _logger;

		public NodeBootStrapper(INodeLifetimeReceiver receiver, ILogger<NodeBootStrapper>? logger = null)
		{
			_receiver = receiver;
			_logger = logger;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await _receiver.InvokeCommandRequestAsync(NodeCommandRequest.OnNodeStarted, cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await _receiver.InvokeCommandRequestAsync(NodeCommandRequest.OnNodeShutdown, cancellationToken);
		}
	}
}
