
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
	public class RenderTreeDefinition : DefinitionBaseType
	{
		public RenderTreeDefinition()
		{
			_layers = new();
		}

		private LayerBaseDefinitionCollection _layers;
		public LayerBaseDefinitionCollection Layers { get => _layers; set => Set(ref _layers, value); }
	}
}
