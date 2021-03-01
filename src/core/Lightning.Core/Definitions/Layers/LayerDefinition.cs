using Lightning.Core.Configuration;
using System.Collections.Generic;

namespace Lightning.Core.Definitions
{
	public class LayerDefinition : LayerBaseDefinition
	{
		public LayerDefinition()
		{
			_input = null!;
			_transformation = new();
			_colorCorrection = new();
			_blendMode = new();
		}

		private InputLayerBaseDefinition _input;
		public InputLayerBaseDefinition Input { get => _input; set => Set(ref _input, value); }

		private TransformProcessorDefinition _transformation;
		public TransformProcessorDefinition Transformation { get => _transformation; set => Set(ref _transformation, value); }

		private ColorProcessorDefinition _colorCorrection;
		public ColorProcessorDefinition ColorCorrection { get => _colorCorrection; set => Set(ref _colorCorrection, value); }

		private MergeProcessorDefinition _blendMode;
		public MergeProcessorDefinition BlendMode { get => _blendMode; set => Set(ref _blendMode, value); }

		protected override ConfigurationChangedTarget Type => throw new System.NotImplementedException();
	}
}
