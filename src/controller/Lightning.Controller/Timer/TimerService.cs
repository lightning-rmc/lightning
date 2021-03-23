using Lightning.Controller.Lifetime;
using Lightning.Controller.Lifetime.Controller;
using Lightning.Controller.Projects;
using Lightning.Core.Lifetime;
using Lightning.Core.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Channels;

namespace Lightning.Controller.Timer
{
	public class TimerService : ICreateOnStartup
	{
		private readonly IControllerStateNotifier _stateNotifier;
		private readonly ILogger<TimerService>? _logger;
		private readonly ControllerSettings _settings;
		private readonly Channel<int> _timerTicksChannel;
		private int _timerTick;

		public TimerService(IControllerStateNotifier controllerStateNotifier,
			IOptions<ControllerSettings> options,
			ILogger<TimerService>? logger = null)
		{
			_stateNotifier = controllerStateNotifier;
			_logger = logger;
			_settings = options.Value;
			_stateNotifier.StateChangeRequested += StateNotifier_StateChangeRequested;
			_timerTicksChannel = Channel.CreateUnbounded<int>();
		}

		public bool IsActive { get; private set; }

		private void StateNotifier_StateChangeRequested(object? sender, StateChangeRequestEventArgs<ControllerState> e)
		{
			if (e.State == ControllerState.Live)
			{
				StartTimer();
			}

			if (e.State is ControllerState.Ready or ControllerState.Shutdown)
			{
				StopTimer();
			}

		}

		private void StartTimer()
		{
			if (!IsActive)
			{
				IsActive = true;
				_ = new Thread(TimerCallback);
				new Thread(() =>
				{
					while (IsActive)
					{
						//Note:The loop has too few instructions
						//     to benefit from a process time calculation with Stopwatch.
						_timerTick++;
						Thread.Sleep((int)(1000f / _settings.TimerInterval));
					}
					_logger?.LogInformation("The timer stopped.");
				}).Start();
			}
		}

		private void StopTimer()
		{
			if (IsActive)
			{
				IsActive = false;
				_timerTicksChannel.Writer.TryWrite(0);
			}
		}


		public IAsyncEnumerable<int> GetTimerTicks()
		{
			return _timerTicksChannel.Reader.ReadAllAsync();
		}

		private void TimerCallback()
		{
			var timerTicks = 0;
			var stopwatch = new Stopwatch();
			var synchronisationInterval = 10;
			stopwatch.Start();
			while (!IsActive)
			{
				_timerTicksChannel.Writer.TryWrite(timerTicks);

				//Calculate next duration
				var duration = stopwatch.ElapsedMilliseconds;
				stopwatch.Reset();
				stopwatch.Start();
				var sleepDuration = (1_000 / synchronisationInterval) - (int)duration;
				sleepDuration = Math.Max(sleepDuration, 0);

				if (_logger?.IsEnabled(LogLevel.Debug) ?? false)
				{
					_logger?.ReportTimerCycle(sleepDuration, duration, timerTicks, DateTime.UtcNow);
				}

				Thread.Sleep(sleepDuration);
			}
		}
	}
}
