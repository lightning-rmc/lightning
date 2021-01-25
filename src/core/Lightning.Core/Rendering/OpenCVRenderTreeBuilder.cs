using Lightning.Core.Presentation;
using Lightning.Core.Rendering.Layers;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering
{
	public class OpenCVRenderTreeBuilder : IRenderTreeBuilder<Mat>
	{
		private readonly IWindowHost<Mat> windowHost;

		public OpenCVRenderTreeBuilder(IWindowHost<Mat> windowHost)
		{
			this.windowHost = windowHost;
		}

		public ILayer<Mat> BuildTree()
		{
			return new DummyLayer(windowHost);
		}
	}
}
