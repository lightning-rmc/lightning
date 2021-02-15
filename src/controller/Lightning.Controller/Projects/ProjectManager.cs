using Lightning.Core.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Projects
{
	public class ProjectManager : IProjectManager
	{
		public string ExportProject()
		{
			return "";
		}

		public LayerDefinition GetLayer(string id)
		{
			return new();
		}

		public NodeDefinition GetNode(string id)
		{
			return new();
		}

		public RenderTreeDefinition GetRenderTree(string id)
		{
			return new RenderTreeDefinition()
			{
				//Root = new LayerDefinition()
				//{
				//	Id = "MyLayer",
				//}
			};
		}

		public bool ImportProject(string import)
		{
			return true;
		}
	}
}
