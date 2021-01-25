using Lightning.Core.Configuration;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
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


		public async IAsyncEnumerable<int> GetTimerTicksAllAsync()
		{
			// Note: Waiting until the Timer is started
			//		 To get sure the stream does not close directly
			while (!IsRunning)
			{
				Cv2.WaitKey(10);
			}

			var stopwatch = new Stopwatch();
			stopwatch.Start();
			while (IsRunning)
			{
				yield return _timerTick;

				//Calculate next duration
				var duration = stopwatch.ElapsedMilliseconds;
				stopwatch.Reset();
				var sleepDuration = (1_000 / _configuration.FramesPerSecond) - (int)duration;
				//Note: for Cv2.WaitKey(), 0 would block the thread until a key-press, use instate 1.
				sleepDuration = Math.Max(sleepDuration, 0);

				if (_logger?.IsEnabled(LogLevel.Debug) ?? false)
				{
					_logger?.LogDebug("Timer run waits duration: '{duration}ms' and inner Timer tick: '{_timerTick}' at time {DateTime.UtcNow}", sleepDuration, _timerTick, DateTime.UtcNow);
				}

				await Task.Delay(sleepDuration);
			}
		}

		public void StartTimer()
		{
			if (!IsRunning)
			{
				//TODO: Logging
				_timerTick = 0;
				IsRunning = true;
				new Thread(() =>
				{
					while (IsRunning)
					{
						//Note:The loop has too few instructions
						//      to benefit from a process time calculation with Stopwatch.
						_timerTick++;
						Thread.Sleep((int)(1000 / (_configuration.FramesPerSecond * _timerInterpolation)));
					}
				}).Start();
			}
			else
			{
				//TODO: Logging
			}
		}

		public void StopTimer()
		{
			if (IsRunning)
			{
				IsRunning = false;
				//TODO: Logging
			}
			else
			{
				//TODO: Logging
			}
		}
	}
}
