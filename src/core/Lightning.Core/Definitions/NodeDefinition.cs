using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public class NodeDefinition
	{
		public NodeDefinition()
		{
			Resolution = new();
		}

		public string Id { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;


		public int FramesPerSecond { get; set; }
		public ResolutionDefinition Resolution { get; set; }

	}
}
