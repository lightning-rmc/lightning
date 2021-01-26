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
	public class RenderHost<TFrame> : IRenderHost
		where TFrame : new()
	{
		private readonly IRenderTimer _timer;
		private readonly IRenderTreeBuilder<TFrame> _treeBuilder;
		private readonly ILogger<RenderHost<TFrame>>? _logger;

		public RenderHost(IRenderTimer timer, IRenderTreeBuilder<TFrame> treeBuilder, ILogger<RenderHost<TFrame>>? logger = null)
		{
			_timer = timer;
			_treeBuilder = treeBuilder;
			_logger = logger;
		}

		public bool IsRunning { get; private set; }

		public void Start()
		{
			//TODO: Maybe Lock for secure, that it starts only one time?
			if (!IsRunning)
			{
				IsRunning = true;
				var layer = _treeBuilder.BuildTree();

				new Thread(Process)
				{
					Priority = ThreadPriority.AboveNormal,
					IsBackground = true
				}.Start(layer);
				_timer.StartTimer();
				_logger?.LogDebug("RenderHost is started.");
			}
			else
			{
				_logger?.LogWarning("RenderHost is already Running.");
			}
		}

		private void Process(object? o)
		{
			var layer = o as ILayer<TFrame> ?? throw new ArgumentException(null, nameof(o));
			InnerProcess(layer).Wait();

			async Task InnerProcess(ILayer<TFrame> root)
			{
				await foreach (var ticks in _timer.GetTimerTicksAllAsync())
				{
					var image = new TFrame();
					root.Process(image, ticks);
				}
			}
		}



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