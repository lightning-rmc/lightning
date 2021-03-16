using Lightning.Core;
using Lightning.Core.Lifetime;
using Lightning.Core.Rendering;
using Lightning.Core.Utils;
using Lightning.Node.Lifetime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCvSharp;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Node.Rendering
{
	internal class OpenCVWindowHost : IWindowHost<Mat>, IDisposable, ICreateOnStartup
	{
		private readonly ILogger<OpenCVWindowHost>? _logger;
		private readonly FeatureFlags _featureFlags;
		private Channel<Mat> _frameChannel = null!;
		private bool _isWindowShowing;
		private Window? _window;

		public OpenCVWindowHost(IOptions<FeatureFlags> featureFlagsOptions,
			INodeStateNotifier nodeLifetime,
			ILogger<OpenCVWindowHost>? logger = null)
		{
			_isWindowShowing = false;
			_logger = logger;
			_featureFlags = featureFlagsOptions.Value;
			nodeLifetime.StateChangeRequested += (s, e) =>
			{
				if (e.State == NodeState.Start)
				{
					e.AddTask(Task.Run(() =>
					{
						ShowWindow();
					}, e.Token));
				}
			};
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
					_logger?.LogDebug("The window will be displayed.");
					while (_isWindowShowing)
					{
						if (_frameChannel.Reader.TryRead(out var frame))
						{
							if (!frame.Empty())
							{
								_window.ShowImage(frame);
							}
							else
							{
								_logger?.LogDebug("Skipped Empty Frame. Empty Frames ca not be displayed.");
							}
							frame.Dispose();
						}
						//TODO: Change to customizable parameter or use the FPS rate
						Cv2.WaitKey(16);
					}

				}).Start();

			}
			else
			{
				_logger?.LogDebug("An attempt was made to open the window, although it is already active.");
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
				_logger?.LogInformation("The displaying of the window was finished.The window was closed.");
			}
			else
			{
				_logger?.LogDebug("An attempt was made to close the window although it is not active.");
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
			else
			{
				_logger?.LogWarning("Attention, it tries to display a frame on the window, although no window is active. The frame will be discarded!");
			}
		}

		public void Dispose()
		{
			HideWindow();
		}
	}
}
