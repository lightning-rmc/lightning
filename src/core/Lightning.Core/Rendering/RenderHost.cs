using Lightning.Core.Configuration;
using Lightning.Core.Rendering.Time;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering
{
	internal abstract class RenderHost<TFrame> : IRenderHost
	{
		private readonly IRenderTimer _timer;
		private readonly IRenderTreeBuilder<TFrame> _treeBuilder;
		private readonly ILogger? _logger;

		public RenderHost(IRenderTimer timer, IRenderTreeBuilder<TFrame> treeBuilder, ILogger? logger = null)
		{
			_timer = timer;
			_treeBuilder = treeBuilder;
			_logger = logger;
		}

		public bool IsRunning { get; private set; }

		public async Task StartAsync()
		{
			//TODO: Maybe Lock for secure, that it starts only one time?
			if (!IsRunning)
			{
				IsRunning = true;
				var layer = await _treeBuilder.BuildTreeAsync();

				new Thread(Process)
				{
					Priority = ThreadPriority.Highest,
					IsBackground = true
				}.Start(layer);
				_logger?.LogDebug("RenderHost is started.");
			}
			else
			{
				_logger?.LogWarning("RenderHost is already Running.");
			}
		}

		private void Process(object? o)
		{
			var root = o as ILayer<TFrame> ?? throw new ArgumentException(null, nameof(o));

			foreach (var ticks in _timer.GetTimerTicks())
			{
				var image = CreateFrame();
				root.Process(image, ticks);
				if (!IsRunning)
				{
					return;
				}
			}
		}
		protected abstract TFrame CreateFrame();


		public void Stop()
		{
			if (IsRunning)
			{
				IsRunning = false;
				_logger?.LogDebug("RenderHost is stopped.");
			}
			else
			{
				_logger?.LogWarning("RenderHost is not Running.");
			}
		}
	}
}
