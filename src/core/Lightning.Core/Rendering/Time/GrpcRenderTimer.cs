using Lightning.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering.Time
{
	public class GrpcRenderTimer : IRenderTimer
	{
		private readonly RenderConfiguration _configuration;
		private Thread _innerClock;
		private float _timerInterpolation;
		private int _timerTick;

		public GrpcRenderTimer(IConfigurationHandler<RenderConfiguration> configurationHandler)
		{
			_configuration = configurationHandler.GetConfiguration();
			_timerInterpolation = 1;
			_timerTick = 0;
		}


		public bool IsRunning { get; private set; }


		public async IAsyncEnumerable<int> GetTimerStream()
		{
			// Note: Waiting until the Timer is started
			//		 To get sure the stream does not close directly
			while (!IsRunning)
			{
				await Task.Delay(10);
			}

			var stopwatch = new Stopwatch();
			stopwatch.Start();
			while (IsRunning)
			{
				yield return _timerTick;
				var duration = stopwatch.ElapsedMilliseconds;
                stopwatch.Reset();
				var sleepDuration = (1000 / _configuration.FramesPerSecond) - (int)duration;
				sleepDuration = Math.Max(sleepDuration,0);
				//TODO: Logging
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
				_innerClock = new Thread(() =>
				{
					while (IsRunning)
					{
						//Note:The loop has too few instructions
						//      to benefit from process time calculation with Stopwatch.
						_timerTick++;
						Thread.Sleep((int)(1000 / (_configuration.FramesPerSecond * _timerInterpolation)));
					}
				});
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
