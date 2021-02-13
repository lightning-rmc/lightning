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
			//Mat image = new Mat();
			var image = new Mat<Vec3b>(); // cv::Mat_<cv::Vec3b>
			_capture.Read(image);
			if (image.Empty())
				return;
			//var indexer = image.GetIndexer();

			//for (int y = 0; y < image.Height; y++)
			//{
			//	for (int x = 0; x < image.Width; x++)
			//	{
			//		Vec3b color = indexer[y, x];
			//		byte temp = color.Item0;
			//		color.Item0 = color.Item2; // B <- R
			//		color.Item2 = temp;        // R <- B
			//		indexer[y, x] = color;
			//	}
			//}
			var flip = new Mat<Vec3b>();
			Cv2.Flip(image, flip, FlipMode.X);

			_windowHost.WriteFrame(flip);
		}
	}
}
