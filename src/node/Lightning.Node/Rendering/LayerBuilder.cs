using Lightning.Core.Definitions.Layers;
using Lightning.Core.Presentation;
using Lightning.Core.Rendering;
using Lightning.Core.Rendering.Layers.Outputs;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node.Rendering
{
	internal static class LayerBuilder
	{


		internal static ILayer<Mat> BuildOutputWindowLayer(IWindowHost<Mat> host, WindowOutputDefinition definition)
			=> new WindowOutputLayer(host, definition);
	}
}
