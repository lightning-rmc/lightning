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
		}
	}
}
