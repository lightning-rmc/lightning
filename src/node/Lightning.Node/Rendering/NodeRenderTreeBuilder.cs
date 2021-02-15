using Lightning.Core.Presentation;
using Lightning.Core.Rendering;
using Lightning.Core.Rendering.Layers;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node.Rendering
{
	internal class NodeRenderTreeBuilder : IRenderTreeBuilder<Mat>
	{
		private readonly IWindowHost<Mat> _windowHost;

		public NodeRenderTreeBuilder(IWindowHost<Mat> windowHost)
		{
			_windowHost = windowHost;
		}

		public async Task<ILayer<Mat>> BuildTreeAsync()
		{
			return new DummyLayer(_windowHost);
		}
	}
}
