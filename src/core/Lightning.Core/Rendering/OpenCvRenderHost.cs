using Lightning.Core.Configuration;
using Lightning.Core.Rendering.Time;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using System;
using System.Diagnostics;
using System.Threading;

namespace Lightning.Core.Rendering
{
    public class OpenCvRenderHost : IRenderHost
    {
		private readonly IRenderTimer _timer;
		private readonly ILogger<OpenCvRenderHost>? _logger;
        private Thread? _renderThread;

        public OpenCvRenderHost(IRenderTimer timer, ILogger<OpenCvRenderHost>? logger = null)
        {
			_timer = timer;
			_logger = logger;
        }

        public bool IsRunning { get; private set; }

        public void Start()
        {
            if (!IsRunning)
            {
                _renderThread = new Thread(Process)
                {
                    Priority = ThreadPriority.AboveNormal,
                    IsBackground = true
                };
                IsRunning = true;
                _renderThread.Start();
				_timer.StartTimer();
                _logger?.LogDebug("RenderHost is started.");
            }
            else
            {
                _logger?.LogWarning("RenderHost is already Running.");
            }
        }

        private void Process()
        {
            using Window window = new("screen");
            using var capture = new VideoCapture("Alone_low.mp4");
			var timerStream = _timer.GetTimerStream().GetEnumerator();
            while (timerStream.MoveNext() && IsRunning)
            {
				var ticks = timerStream.Current;

                using var image = new Mat();
                capture.Read(image);
                if (image.Empty())
                    break;

                window.ShowImage(image);
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
