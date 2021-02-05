using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions.Layers
{
	public class LayerDefinition : LayerBaseDefinition
	{

		public InputLayerBaseDefinition Input { get; set; } = null!;

		public TransformProcessorDefinition Transformation { get; set; } = new TransformProcessorDefinition();
		public ColorProcessorDefinition ColorCorrection { get; set; } = new ColorProcessorDefinition();
		public MergeProcessorDefinition BlendMode { get; set; } = new MergeProcessorDefinition();
		public LayerBaseDefinition Child { get; set; } = null!;

		public override IEnumerable<LayerBaseDefinition> GetChilds()
		{
			yield return Child;
		}
	}
}
