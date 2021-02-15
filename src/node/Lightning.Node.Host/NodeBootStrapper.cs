using Lightning.Core.Rendering;
using Lightning.Core.Presentation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Lightning.Node.Communications;

namespace Lightning.Node.Host
{
	public class NodeBootStrapper : IHostedService
	{
		private readonly IWindowHost _windowHost;
		private readonly IConnectionManager _connectionManager;
		private readonly ILogger<NodeBootStrapper>? _logger;

		public NodeBootStrapper(IWindowHost windowHost, IConnectionManager connectionManager,ILogger<NodeBootStrapper>? logger = null)
		{
			_windowHost = windowHost;
			_connectionManager = connectionManager;
			_logger = logger;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_windowHost.ShowWindow();
			await _connectionManager.SearchAndAuthenticateForServerAsync();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_windowHost.Dispose();
			return Task.CompletedTask;
		}
	}
}
