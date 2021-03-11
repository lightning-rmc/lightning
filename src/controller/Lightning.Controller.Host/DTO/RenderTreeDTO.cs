using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.DTO
{
	public class RenderTreeDTO
	{
		public string Id { get; set; } = string.Empty;
		public IEnumerable<LayerDTO> Layers { get; set; } = Enumerable.Empty<LayerDTO>();
	}
}
