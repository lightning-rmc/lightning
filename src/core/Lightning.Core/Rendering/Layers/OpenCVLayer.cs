using Lightning.Core.Definitions;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering.Layers
{
	public class OpenCVLayer : LayerBase<Mat>
	{
		private readonly LayerDefinition _definition;
		private readonly ILayer<Mat>? _child;

		public OpenCVLayer(LayerDefinition definition, ILayer<Mat>? child = null)
			: base(definition.Id)
		{
			_definition = definition;
			_child = child;
		}

		protected override Mat InternalProcess(Mat frame, int tick)
		{
			//TODO: add more Process Information
			return frame;
		}

		protected override void ProcessChilds(Mat frame, int tick)
			=> _child?.Process(frame, tick);
	}
}
