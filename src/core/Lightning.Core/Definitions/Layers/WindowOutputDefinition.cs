using Lightning.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions.Layers
{
	public class WindowOutputDefinition : LayerBaseDefinition
	{
		protected override ConfigurationChangedTarget Type
			=> ConfigurationChangedTarget.Output;
	}
}
