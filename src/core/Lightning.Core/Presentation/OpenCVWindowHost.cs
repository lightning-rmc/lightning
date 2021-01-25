using Microsoft.Extensions.Logging;
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
		private Channel<Mat> _frameChannel;
		private bool _isWindowShowing;
		private Window _window;

		public OpenCVWindowHost(ILogger<OpenCVWindowHost>? logger = null)
		{
			_isWindowShowing = false;
			_logger = logger;

		}



		public void ShowWindow()
		{
			if (!_isWindowShowing)
			{
				_isWindowShowing = true;
				_frameChannel = Channel.CreateUnbounded<Mat>(new UnboundedChannelOptions
				{
					SingleReader = true
				});
				//Note: Opencv needs a custome Ui thread, we pipe the mat frame with the channel into the ui thread
				new Thread(() =>
				{
					_window = new Window("lighting_window", WindowMode.Normal);
					_window.SetProperty(WindowProperty.Fullscreen, 1);
					WriteFrame(Mat.Zeros(new Size(1, 1), MatType.CV_16SC3));
					//TODO: add Logging
					while (_isWindowShowing)
					{
						if (_frameChannel.Reader.TryRead(out var frame))
						{
							_window.ShowImage(frame);
							frame.Dispose();
						}
						//TODO: Change to customizable parameter or use the FPS rate
						Cv2.WaitKey(1);
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
