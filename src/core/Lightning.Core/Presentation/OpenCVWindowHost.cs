using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Core.Presentation
{
	public class OpenCVWindowHost : IWindowHost<Mat>, IDisposable
	{
		private readonly ILogger<OpenCVWindowHost>? _logger;
		private readonly FeatureFlags _featureFlags;
		private readonly Queue<Action> _dispatcherQueue;
		private Channel<Mat> _frameChannel;
		private bool _isWindowShowing;
		private Window _window;

		public OpenCVWindowHost(IOptions<FeatureFlags> featureFlagsOptions, ILogger<OpenCVWindowHost>? logger = null)
		{
			_isWindowShowing = false;
			_logger = logger;
			_featureFlags = featureFlagsOptions.Value;
			_dispatcherQueue = new Queue<Action>();
		}



		public void ShowWindow()
		{
			if (!_isWindowShowing)
			{
				_isWindowShowing = true;
				_frameChannel = Channel.CreateUnbounded<Mat>(new UnboundedChannelOptions
				{
					SingleReader = true,
					//Note: Not sure if it is a single Writer? Maybe not
					//SingleWriter = true 
				});

				//Note: Opencv needs a own Ui thread, we pipe the mat frame with a channel into the ui thread.
				//		For decoupling it from the renderhost and make it independent
				new Thread(() =>
				{
					_window = new Window("lighting_window", WindowMode.Normal);
					if (!_featureFlags.DisableFullscreen)
					{
						_window.SetProperty(WindowProperty.Fullscreen, 1);
					}
					WriteFrame(Mat.Zeros(new Size(1, 1), MatType.CV_16SC3));
					//TODO: add Logging
					Action callback;
					while (_isWindowShowing)
					{
						if (_frameChannel.Reader.TryRead(out var frame))
						{
							_window.ShowImage(frame);
							frame.Dispose();
						}
						//if (_dispatcherQueue.TryDequeue(out callback))
						//{
						//	callback();
						//}
						//TODO: Change to customizable parameter or use the FPS rate
						Cv2.WaitKey(14);
					}

				}).Start();

			}

		}

		public void HideWindow()
		{
			if (_isWindowShowing)
			{
				_isWindowShowing = false;
				_frameChannel.Writer.Complete();
				_window.Close();
				if (!_window.IsDisposed)
				{
					_window.Dispose();
				}
				//TODO: add Logging
			}
		}

		public void WriteFrame(Mat mat)
		{
			if (_isWindowShowing)
			{
				//TODO: Maybe add Resize logic

				//Note: can ignore if the write is successful
				//_dispatcherQueue.Enqueue(() =>
				//{
				//	_window.ShowImage(mat);
				//	mat.Dispose();
				//});
				_frameChannel.Writer.TryWrite(mat);
			}
			//TODO: add logging
		}

		public void Dispose()
		{
			HideWindow();
		}
	}
}
