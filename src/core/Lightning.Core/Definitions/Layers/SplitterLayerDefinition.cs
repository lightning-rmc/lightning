using Lightning.Core.Definitions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public class SplitterLayerDefinition : LayerBaseDefinition
	{
		public SplitterLayerDefinition()
		{
			Childs = new();
		}


		public LayerBaseDefinitionCollection Childs { get; set; }

		public override IEnumerable<LayerBaseDefinition> GetChilds()
			=> Childs;
	}
}
