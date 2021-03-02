using Lightning.Core.Definitions.Layers;
using Lightning.Core.Presentation;
using Lightning.Core.Rendering;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node.Rendering
{
	public class WindowHostOutputLayer : ILayer<Mat>
	{
		private readonly WindowOutputDefinition _definition;
		private readonly IWindowHost<Mat> _windowHost;

		public WindowHostOutputLayer(WindowOutputDefinition definition, IWindowHost<Mat> windowHost)
		{
			_definition = definition;
			_windowHost = windowHost;
		}

		public string Name => _definition.Id;
		public bool IsActive { get; set; }

		public void Process(Mat frame, int tick)
		{
			_windowHost.WriteFrame(frame);
		}

		public void Dispose()
		{
			//Note: no need for disposing
		}
	}
}
