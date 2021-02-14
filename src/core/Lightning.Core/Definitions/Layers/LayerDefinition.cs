using System.Collections.Generic;

namespace Lightning.Core.Definitions
{
	public class LayerDefinition : LayerBaseDefinition
	{
		public LayerDefinition()
		{
			Transformation = new();
			ColorCorrection = new();
			BlendMode = new();
		}

		public InputLayerBaseDefinition Input { get; set; } = null!;

		public TransformProcessorDefinition Transformation { get; set; }
		public ColorProcessorDefinition ColorCorrection { get; set; }
		public MergeProcessorDefinition BlendMode { get; set; }

		public LayerBaseDefinition Child { get; set; } = null!;

		public override IEnumerable<LayerBaseDefinition> GetChilds()
		{
			yield return Child;
		}
	}
}
