using Lightning.Core.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Projects
{
	public interface IProjectManager
	{
		string ExportProject();
		bool ImportProject(string import);

		IEnumerable<RenderTreeDefinition> GetRenderTreeDefinitions();
		IEnumerable<LayerDefinition> GetLayers();
		IEnumerable<NodeDefinition> GetNodes();

		RenderTreeDefinition GetRenderTree(string id);
		LayerDefinition GetLayer(string id);
		NodeDefinition GetNode(string id);
	}
}
