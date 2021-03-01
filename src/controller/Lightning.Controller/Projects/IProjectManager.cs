using Lightning.Core.Configuration;
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

		bool IsProjectLoaded { get; }
		string? ExportProject();
		void CreateNewProject();
		bool ImportProject(string import);

		RenderTreeDefinition? TryGetRenderTree(string id);

		NodeDefinition? TryGetNode(string id);

		LayerBaseDefinition? TryGetLayer(string id);

<<<<<<< HEAD
		IAsyncEnumerable<ConfigurationChangedContext> GetConfigurationChangedAllAsync(CancellationToken cancellationToken = default);

=======
		IEnumerable<RenderTreeDefinition> GetRenderTreeDefinitions();
		IEnumerable<LayerDefinition> GetLayers();
		IEnumerable<NodeDefinition> GetNodes();

		RenderTreeDefinition GetRenderTree(string id);
		LayerDefinition GetLayer(string id);
		NodeDefinition GetNode(string id);
>>>>>>> 86d4e994f08e96b91aea8ca72fde9393ba71e00f
	}
}
