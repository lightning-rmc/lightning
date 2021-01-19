using Lightning.Core.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using System;
using System.Diagnostics;
using System.Threading;

namespace Lightning.Core.Rendering
{
    public class RenderHost : IRenderHost
    {
        private readonly RenderConfiguration _configuration;
        private readonly ILogger<RenderHost>? _logger;
        private Thread? _renderThread;

        public RenderHost(IConfigurationHandler<RenderConfiguration> configurationHandler, ILogger<RenderHost>? logger = null)
        {
            _configuration = configurationHandler.GetConfiguration();
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
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (IsRunning)
            {

                using var image = new Mat();
                capture.Read(image);
                if (image.Empty())
                    break;

                window.ShowImage(image);

                var duration = stopwatch.ElapsedMilliseconds;
                stopwatch.Reset();
                var sleepDuration = (1000 / _configuration.FramesPerSecond) - (int)duration;
                sleepDuration = sleepDuration > 0 ? sleepDuration : 1;
                Console.WriteLine(sleepDuration);
                Cv2.WaitKey(sleepDuration);
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



        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            // TODO: Maybe later check for mats to dispose. 
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        ~RenderHost() => Dispose(disposing: false);
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
