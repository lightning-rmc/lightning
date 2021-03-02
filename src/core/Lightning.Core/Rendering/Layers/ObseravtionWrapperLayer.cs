using OpenCvSharp;
using System;

namespace Lightning.Core.Rendering.Layers
{
	public class ObseravtionWrapperLayer : ILayer<Mat>
	{
		private readonly LayerActivationObserver<Mat> _layerActivationObserver;
		private readonly ILayer<Mat> _child;

		public ObseravtionWrapperLayer(LayerActivationObserver<Mat> layerActivationObserver, ILayer<Mat> child)
		{
			_layerActivationObserver = layerActivationObserver;
			_child = child;
		}

		public string Name => string.Empty;

		public bool IsActive { get; set; }

		public void Dispose()
		{
			_layerActivationObserver.Dispose();
		}

		public void Process(Mat frame, int tick)
		{
			_child.Process(frame, tick);
		}

		
	}
}
