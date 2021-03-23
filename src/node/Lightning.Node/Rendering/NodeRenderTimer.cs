using Grpc.Core;
using Lightning.Core.Definitions;
using Lightning.Core.Rendering.Time;
using Lightning.Core.Utils;
using Lightning.Node.Communications;
using Microsoft.Extensions.Logging;
using Portable.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Node.Rendering
{
	public class NodeRenderTimer : IRenderTimer
	{
		private readonly IConnectionManager _connectionManager;
		private readonly ILogger<NodeRenderTimer>? _logger;
		private TimerDefinition? _timerDefinition = null;
		private float _timerInterpolation;
		private int _timerTick;
		private int? _grpcTick;
		private CancellationTokenSource? _grpcCts;

		public NodeRenderTimer(IConnectionManager connectionManager, ILogger<NodeRenderTimer>? logger = null)
		{
			_timerInterpolation = 1;
			_timerTick = 0;
			_connectionManager = connectionManager;
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
			if (_timerDefinition is null)
			{
				throw new InvalidOperationException();
			}

			var stopwatch = new Stopwatch();
			stopwatch.Start();
			while (IsRunning)
			{
				yield return _timerTick;

				//Calculate next duration
				var duration = stopwatch.ElapsedMilliseconds;
				stopwatch.Reset();
				stopwatch.Start();
				var sleepDuration = (1_000 / _timerDefinition.DefaultFrameRate) - (int)duration;
				//Note: for Cv2.WaitKey(), 0 would block the thread until a key-press, use instate 1.
				sleepDuration = Math.Max(sleepDuration, 0);

				if (_logger?.IsEnabled(LogLevel.Debug) ?? false)
				{
					_logger?.ReportTimerCycle(sleepDuration, duration, _timerTick, DateTime.UtcNow);
				}

				Thread.Sleep(sleepDuration);
			}
		}

		public async Task StartTimerAsync()
		{
			if (!IsRunning)
			{
				_logger?.LogInformation("The timer started.");
				_timerTick = 0;
				var httpClient = _connectionManager.GetHttpClient();
				var response = await httpClient.GetAsync($"api/layergroups/timerfornode/{_connectionManager.NodeId}");
				//TODO: Handle response code...
				var stringResponse = await response.Content.ReadAsStringAsync();
				if (XamlServices.Parse(stringResponse) is not TimerDefinition timerDefinition)
				{
					//TODO: add logging and meaningful exceptionmessage
					throw new InvalidOperationException();
				}
				_timerDefinition = timerDefinition;
				IsRunning = true;
				new Thread(TimerClock).Start();
				_grpcCts = new CancellationTokenSource();
				_ = Task.Run(async () => await SychroniseTimersAsync(_grpcCts.Token), _grpcCts.Token);
			}
			else
			{
				_logger?.LogWarning("The timer has already started. The call will be ignored. Should never happen.");
			}
		}

		private void TimerClock()
		{
			while (IsRunning)
			{
				//Note:The loop has too few instructions
				//     to benefit from a process time calculation with Stopwatch.
				if (_grpcTick is not null)
				{
					var tempTick = _grpcTick.Value;
					if (Math.Abs(tempTick - _timerTick) > 20)
					{
						_logger?.LogDebug("Controller tick and node tick getting synchronized, controller tick: '{ctick}' node tick: '{ntick}'", tempTick, _timerTick);
						_timerTick = _grpcTick.Value;
					}
					_grpcTick = null;
				}
				else
				{
					_timerTick++;
				}
				Thread.Sleep((int)(1000 / (_timerDefinition!.DefaultFrameRate * _timerInterpolation)));
			}
			_logger?.LogInformation("The timer stopped.");
		}

		public void StopTimer()
		{
			if (IsRunning)
			{
				IsRunning = false;
				_grpcCts?.Cancel();
			}
			else
			{
				_logger?.LogWarning("The timer has already stopped. The call will be ignored.");
			}
		}

		private async Task SychroniseTimersAsync(CancellationToken token = default)
		{
			var client = _connectionManager.GetTimeServiceClient();
			var stream = client.GetSychronisationStream(new(), cancellationToken: token);

			await foreach (var timeStamp in stream.ResponseStream.ReadAllAsync(token))
			{
				_logger?.LogDebug("New timer tick from controller tick: '{tick}'.", timeStamp.Tick);
				_grpcTick = timeStamp.Tick;
			}
		}
	}
}
