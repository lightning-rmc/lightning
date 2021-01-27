using Lightning.Core.Presentation;
using OpenCvSharp;

namespace Lightning.Core.Rendering.Layers
{
	internal class DummyLayer : ILayer<Mat>
	{
		private readonly IWindowHost<Mat> _windowHost;
		private readonly VideoCapture _capture;
		public DummyLayer(IWindowHost<Mat> windowHost)
		{
			//_capture = new VideoCapture("Alone_low.mp4");
			//_capture = new VideoCapture("Waterfall - 37088.mp4");
			_capture = new VideoCapture("Waterfall - 37088 (1).mp4");
			//_capture = new VideoCapture("Waterfall - 37088_2.mp4");
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
