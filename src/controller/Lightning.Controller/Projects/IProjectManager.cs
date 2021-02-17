using Lightning.Core.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Controller.Projects
{
	public interface IProjectManager
	{
		event EventHandler ProjectLoaded;

		void CreateNewProject();
		string ExportProject();
		bool ImportProject(string import);

		RenderTreeDefinition GetRenderTree(string id);

		LayerDefinition GetLayer(string id);
		NodeDefinition GetNode(string id);

		IAsyncEnumerable<LayerPropertyUpdate> GetLayerPropertyUpdatesAllAsync(CancellationToken cancellationToken = default);

	}
}
