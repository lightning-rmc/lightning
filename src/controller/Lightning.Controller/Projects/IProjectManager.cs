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
		void ImportProject(ProjectDefinition import);

		RenderTreeDefinition? TryGetRenderTree(string renderTreeId);
		RenderTreeDefinition? TryGetRenderTreeForNode(string nodeId);

		NodeDefinition? TryGetNode(string id);

		LayerBaseDefinition? TryGetLayer(string id);

		IEnumerable<NodeDefinition> GetNodes();
		IEnumerable<RenderTreeDefinition> GetRenderTrees();

		RenderTreeDefinition? TryAddRenderTree();

		IAsyncEnumerable<ConfigurationChangedContext> GetConfigurationChangedAllAsync(CancellationToken cancellationToken = default);

	}
}
