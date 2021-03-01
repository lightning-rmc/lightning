using Lightning.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public class TransformProcessorDefinition : DefinitionBaseType
	{
		public TransformProcessorDefinition()
		{

		}

		protected override ConfigurationChangedTarget Type
			=> ConfigurationChangedTarget.Transition;
	}
}
