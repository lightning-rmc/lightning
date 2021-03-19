using Lightning.Core.Configuration;
using Lightning.Core.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering.Time
{
	public class RenderTimer : IRenderTimer
	{
		private readonly RenderConfiguration _configuration;
		private readonly ILogger<RenderTimer>? _logger;
		private float _timerInterpolation;
		private int _timerTick;

		public RenderTimer(IConfigurationHandler<RenderConfiguration> configurationHandler, ILogger<RenderTimer>? logger = null)
		{
			_configuration = configurationHandler.GetConfiguration();
			_timerInterpolation = 1;
			_timerTick = 0;
			_logger = logger;
		}


		public bool IsRunning { get; private set; }


		public IEnumerable<int> GetTimerTicks()
		{
			// Note: Waiting until the Timer is started
			//		 To get sure the stream does not close directly
			_logger?.LogDebug("'GetTimerTicks' has been called it waits blocking until the timer has stopped!");
			while (!IsRunning)
			{
				Thread.Sleep(10);
			}
			_logger?.LogDebug("The timer is started, 'GetTimerTicks' does not block anymore!");

			var stopwatch = new Stopwatch();
			stopwatch.Start();
			while (IsRunning)
			{
				yield return _timerTick;

				//Calculate next duration
				var duration = stopwatch.ElapsedMilliseconds;
				stopwatch.Reset();
				stopwatch.Start();
				var sleepDuration = (1_000 / _configuration.FramesPerSecond) - (int)duration;
				//Note: for Cv2.WaitKey(), 0 would block the thread until a key-press, use instate 1.
				sleepDuration = Math.Max(sleepDuration, 0);

				if (_logger?.IsEnabled(LogLevel.Debug) ?? false)
				{
					_logger?.ReportTimerCycle(sleepDuration, duration, _timerTick, DateTime.UtcNow);
				}

				Thread.Sleep(sleepDuration);
			}
		}

		public void StartTimer()
		{
			if (!IsRunning)
			{
				_logger?.LogInformation("The timer started.");
				_timerTick = 0;
				IsRunning = true;
				new Thread(() =>
				{
					while (IsRunning)
					{
						//Note:The loop has too few instructions
						//     to benefit from a process time calculation with Stopwatch.
						_timerTick++;
						Thread.Sleep((int)(1000 / (_configuration.FramesPerSecond * _timerInterpolation)));
					}
					_logger?.LogInformation("The timer stopped.");
				}).Start();
			}
			else
			{
				_logger?.LogWarning("The timer has already started. The call will be ignored.");
			}
		}

		public void StopTimer()
		{
			if (IsRunning)
			{
				IsRunning = false;
			}
			else
			{
				_logger?.LogWarning("The timer has already stopped. The call will be ignored.");
			}
		}
	}
}
