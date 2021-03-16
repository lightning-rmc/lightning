using Lightning.Core.Definitions;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering.Layers.Outputs
{
	public class WindowOutputLayer : ILayer<Mat>
	{
		private readonly IWindowHost<Mat> _windowHost;
		private readonly WindowOutputDefinition _definition;

		public WindowOutputLayer(IWindowHost<Mat> windowHost, WindowOutputDefinition definition)
		{
			_windowHost = windowHost;
			_definition = definition;
		}

		public string Name => _definition.Id;

		public bool IsActive { get; set; }

		public void Dispose()
		{
			//Note: Nothing to handle
		}

		public void Process(Mat frame, int tick)
		{
			_windowHost.WriteFrame(frame);
		}
	}
}
