using Lightning.Controller.Lifetime;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Controller.Host
{
	public class StartupBootStrapper : IHostedService
	{
		private readonly INodeLifetimeService _nodeLifetimeService;

		public StartupBootStrapper(INodeLifetimeService nodeLifetimeService)
		{
			_nodeLifetimeService = nodeLifetimeService;
		}
		public Task StartAsync(CancellationToken cancellationToken)
		{
			_nodeLifetimeService.TryRegisterNode("test");
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}
