using Lightning.Core.Configuration;

namespace Lightning.Core.Definitions
{
	public class ColorProcessorDefinition : DefinitionBaseType
	{
		protected override ConfigurationChangedTarget Type
			=> ConfigurationChangedTarget.ColorCorrection;
	}
}
