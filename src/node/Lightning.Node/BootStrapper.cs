using Lightning.Core.Rendering;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Node
{
    public class BootStrapper : IHostedService
    {
        private readonly IRenderHost _renderHost;
        private readonly ILogger<BootStrapper>? _logger;

        public BootStrapper(IRenderHost renderHost, ILogger<BootStrapper>? logger = null)
        {
            _renderHost = renderHost;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _renderHost.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _renderHost.Stop();
            return Task.CompletedTask;
        }
    }
}
