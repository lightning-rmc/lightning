
using Lightning.Core.Definitions.Collections;
using Portable.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	[ContentProperty(nameof(Layers))]
	public class RenderTreeDefinition
	{
		public RenderTreeDefinition()
		{
			Layers = new();
		}

		public LayerBaseDefinitionCollection Layers { get; set; }
	}
}
