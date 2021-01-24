using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Projects.Structure
{
	public class ProjectInfo
	{
		public ProjectInfo()
		{
			Nodes = new List<NodeInfo>();
		}

		public List<NodeInfo> Nodes { get; set; }


	}
}
