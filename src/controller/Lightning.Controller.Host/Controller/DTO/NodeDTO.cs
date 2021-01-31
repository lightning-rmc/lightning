using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.Controller.DTO
{
	public class NodeDTO
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public NodeState State { get; set; }
	}
}
