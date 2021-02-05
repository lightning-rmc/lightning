using Lightning.Core.Presentation;
using OpenCvSharp;

namespace Lightning.Core.Rendering.Layers
{
	public class DummyLayer : ILayer<Mat>
	{
		private readonly IWindowHost<Mat> _windowHost;
		private readonly VideoCapture _capture;
		public DummyLayer(IWindowHost<Mat> windowHost)
		{

			_capture = new VideoCapture("media/video_fullhd.mp4");
			_windowHost = windowHost;
		}
		public string Name => string.Empty;

		public bool IsActive => true;

		public void Process(Mat frame, int tick)
		{
			Mat image = new Mat();
			_capture.Read(image);
			if (image.Empty())
				return;
			_windowHost.WriteFrame(image);
		}
	}
}
