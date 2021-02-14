using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public abstract class LayerBaseDefinition
	{

		protected LayerBaseDefinition()
		{

		}

		public string Id { get; set; } = string.Empty;


		public abstract IEnumerable<LayerBaseDefinition> GetChilds();

	}
}
