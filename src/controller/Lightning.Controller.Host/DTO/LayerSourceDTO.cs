using Lightning.Core.Definitions.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.DTO
{
	public class LayerSourceDTO
	{
		public string Source { get; set; } = string.Empty;

		public static LayerSourceDTO FromDefinition(FileInputLayerDefinition def)
		{
			return new()
			{
				Source = def.Filename
			};
		}
	}
}
