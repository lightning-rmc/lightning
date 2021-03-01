using Lightning.Node.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node
{
	public class NodeConfiguration
	{
		public string NodeId { get; set; } = string.Empty;
		public MediaConfiguration Media { get; set; } = new();
	}
}
