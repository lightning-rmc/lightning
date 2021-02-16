using Lightning.Core.Definitions.Collections;
using Portable.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	[ContentProperty(nameof(Childs))]
	public class SplitLayerDefinition : LayerBaseDefinition
	{
		public SplitLayerDefinition()
		{
			Childs = new();
		}


		public LayerBaseDefinitionCollectionCollection Childs { get; set; }
	}
}
