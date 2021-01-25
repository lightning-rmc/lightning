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
	public class OpenCvRenderHost : IRenderHost
	{
		private readonly IRenderTimer _timer;
		private readonly IRenderTreeBuilder<Mat> _treeBuilder;
		private readonly ILogger<OpenCvRenderHost>? _logger;

		public OpenCvRenderHost(IRenderTimer timer, IRenderTreeBuilder<Mat> treeBuilder, ILogger<OpenCvRenderHost>? logger = null)
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
			var layer = o as ILayer<Mat> ?? throw new ArgumentException(null, nameof(o));
			InnerProcess(layer).Wait();

			async Task InnerProcess(ILayer<Mat> root)
			{
				await foreach (var ticks in _timer.GetTimerTicksAllAsync())
				{
					using var image = new Mat();
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
