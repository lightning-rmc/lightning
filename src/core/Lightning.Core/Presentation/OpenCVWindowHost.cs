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
using System.Xaml;

namespace Lightning.Core.Presentation
{
	internal class OpenCVWindowHost : IWindowHost<Mat>, IDisposable
	{
		private readonly ILogger<OpenCVWindowHost>? _logger;
		private readonly FeatureFlags _featureFlags;
		private Channel<Mat> _frameChannel = null!;
		private bool _isWindowShowing;
		private Window? _window;

		public OpenCVWindowHost(IOptions<FeatureFlags> featureFlagsOptions, ILogger<OpenCVWindowHost>? logger = null)
		{
			_isWindowShowing = false;
			_logger = logger;
			_featureFlags = featureFlagsOptions.Value;

		}



		public void ShowWindow()
		{
			if (!_isWindowShowing)
			{
				_isWindowShowing = true;
				//Note: Should never happen, only for secure purpose
				if (_frameChannel is not null)
				{
					_frameChannel.Writer.TryComplete();
				}
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
					while (_isWindowShowing)
					{
						if (_frameChannel.Reader.TryRead(out var frame))
						{
							if (!frame.Empty())
							{
								_window.ShowImage(frame);
							}
							frame.Dispose();
						}
						//TODO: Change to customizable parameter or use the FPS rate
						Cv2.WaitKey(16);
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
				_window?.Close();
				if (!(_window?.IsDisposed ?? true))
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

				//Note: can ignore the result if it is successful
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
