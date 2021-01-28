using Lightning.Core.Rendering;
using Lightning.Core.Presentation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Node.Host
{
	public class NodeBootStrapper : IHostedService
	{
		private readonly IRenderHost _renderHost;
		private readonly IWindowHost _windowHost;
		private readonly ILogger<NodeBootStrapper>? _logger;

		public NodeBootStrapper(IWindowHost windowHost, ILogger<NodeBootStrapper>? logger = null)
		{
			_windowHost = windowHost;
			_logger = logger;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_windowHost.ShowWindow();
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_windowHost.Dispose();
			_renderHost.Stop();
			return Task.CompletedTask;
		}
	}
}
