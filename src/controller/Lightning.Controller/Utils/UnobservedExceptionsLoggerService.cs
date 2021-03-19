using Lightning.Core.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Utils
{
	public class UnobservedExceptionsLoggerService : ICreateOnStartup
	{
		private readonly ILogger<UnobservedExceptionsLoggerService> _logger;

		public UnobservedExceptionsLoggerService(ILogger<UnobservedExceptionsLoggerService> logger)
		{
			_logger = logger;

			TaskScheduler.UnobservedTaskException += (s, e) =>
			{
				_logger?.LogError(e.Exception,"Uncaught exception was thrown in a unobserved Task.");
			};
			AppDomain.CurrentDomain.UnhandledException += (s, e) =>
			{
				_logger?.LogCritical(e.ExceptionObject as Exception, "Uncaught exception was thrown. The runtime will terminate:{terminate}", e.IsTerminating);
			};
		}
	}
}
