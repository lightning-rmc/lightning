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
		private readonly ILayerInput<Mat> _input;

		public OpenCVLayer(LayerDefinition definition, ILayerInput<Mat> input, ILayer<Mat>? child = null)
			: base(definition.Id)
		{
			_definition = definition;
			_child = child;
			_input = input;
			IsActive = true;
		}


		public override bool IsActive
		{
			get => base.IsActive;
			set
			{
				if (value && value != base.IsActive)
				{
					_input.Reset();
				}
				base.IsActive = value;
			}
		}

		protected override Mat InternalProcess(Mat frame, int tick)
		{
			//TODO: add color, Blend, Transform etc...
			return _input.Process(tick);
		}

		protected override void ProcessChilds(Mat frame, int tick)
			=> _child?.Process(frame, tick);
	}
}
