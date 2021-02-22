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

		public IEnumerable<LayerDefinition> GetLayers()
		{
			return Enumerable.Empty<LayerDefinition>();
		}

		public NodeDefinition GetNode(string id)
		{
			return new();
		}

		public IEnumerable<NodeDefinition> GetNodes()
		{
			return Enumerable.Empty<NodeDefinition>();
		}

		public RenderTreeDefinition GetRenderTree(string id)
		{
			return new RenderTreeDefinition()
			{
				//Root = new LayerDefinition()
				//{
				//	Id = "MyLayer",
				//}
				Layers = new()
				{
					new LayerDefinition
					{
						Id = "Layer1"
					}
				}
			};
		}

		public IEnumerable<RenderTreeDefinition> GetRenderTreeDefinitions()
		{
			return Enumerable.Empty<RenderTreeDefinition>();
		}

		public bool ImportProject(string import)
		{
			return true;
		}
	}
}
