using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Core.Utils
{
	public class CreateOnStarupBootstrapper : IHostedService
	{

		public CreateOnStarupBootstrapper(IEnumerable<ICreateOnStartup> _) { }

		public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}
