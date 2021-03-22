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

		LayerGroupDefinition? TryGetLayerGroup(string renderTreeId);
		LayerGroupDefinition? TryGetLayerGroupForNode(string nodeId);

		NodeDefinition? TryGetNode(string id);
		bool TryAddNode(string id);

		LayerBaseDefinition? TryGetLayer(string id);

		IEnumerable<NodeDefinition> GetNodes();
		IEnumerable<LayerGroupDefinition> GetLayerGroups();

		LayerGroupDefinition? TryAddLayerGroup();

		bool TryRemoveLayerGroup(string renderTreeId);
		bool TryRemoveLayer(string layerId);

		IAsyncEnumerable<ConfigurationChangedContext> GetConfigurationChangedAllAsync(CancellationToken cancellationToken = default);

	}
}
