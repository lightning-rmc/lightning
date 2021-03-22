using Lightning.Core.Rendering.Time;
using Microsoft.Extensions.Logging;
using OpenCvSharp;

namespace Lightning.Core.Rendering
{
	internal class OpenCVRenderHost : RenderHost<Mat>
	{
		public OpenCVRenderHost(IRenderTimer timer, IRenderTreeBuilder<Mat> builder, ILogger<OpenCVRenderHost>? logger = null)
			: base(timer, builder, logger)
		{

		}

		protected override Mat CreateFrame()
		{
			return Mat.Zeros(new Size(1, 1), MatType.CV_16SC3);
		}
	}
}
