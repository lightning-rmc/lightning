
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
			_id = string.Empty;
		}

		private string _id;
		public string Id
		{
			get => _id;
			set => Set(ref _id, value);
		}

		private LayerBaseDefinitionCollection _layers;
		public LayerBaseDefinitionCollection Layers { get => _layers; set => Set(ref _layers, value); }

		public LayerBaseDefinition? TryGetLayer(string id)
		{
			return Recursion(Layers, id);
			LayerBaseDefinition? Recursion(IEnumerable<LayerBaseDefinition> layers, string id)
			{
				foreach (var layer in Layers)
				{
					if (layer.Id == id)
					{
						return layer;
					}
					if (layer is SplitLayerDefinition splitLayer)
					{
						foreach (var layerlist in splitLayer.Childs)
						{
							var result = Recursion(layerlist, id);
							if (result is not null)
							{
								return result;
							}
						}
					}
				}
				return null;
			}
		}
	}
}
