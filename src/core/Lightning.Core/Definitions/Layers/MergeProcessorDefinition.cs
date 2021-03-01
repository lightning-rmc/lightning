using Lightning.Core.Configuration;

namespace Lightning.Core.Definitions
{
	public class MergeProcessorDefinition : DefinitionBaseType
	{
		protected override ConfigurationChangedTarget Type
			=> ConfigurationChangedTarget.BlendMode;
	}
}
