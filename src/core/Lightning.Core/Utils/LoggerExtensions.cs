using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Utils
{
	public static class LoggerExtensions
	{

		private static readonly Action<ILogger, int, long, int, DateTime, Exception?> _reportTimerCycle =
			LoggerMessage.Define<int, long, int, DateTime>(LogLevel.Debug, new EventId(2, nameof(ReportTimerCycle)),
				"Timer run, sleepDuration: '{sleepDuration}ms', calcDuration '{duration}ms'  inner Timer tick: '{_timerTick}' at time {DateTime.UtcNow}");




		public static void ReportTimerCycle(this ILogger logger, int sleepDuration, long duration, int ticks, DateTime date)
		{
			_reportTimerCycle(logger, sleepDuration, duration, ticks, date, null);
		}
	}
}
