using Lightning.Core.Configuration;
using Lightning.Core.Definitions;
using Microsoft.Extensions.Logging;
using Portable.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;

namespace Lightning.Controller.Projects
{
	public class ProjectManager : IProjectManager
	{
		public event EventHandler? ProjectLoaded;
		private readonly ILogger<ProjectManager>? _logger;
		private readonly Channel<ConfigurationChangedContext> _configurationsChangedChannel;
		private ProjectDefinition? _project;

		public ProjectManager(ILogger<ProjectManager>? logger = null)
		{
			_logger = logger;
			_configurationsChangedChannel = Channel.CreateUnbounded<ConfigurationChangedContext>();
		}

		public bool IsProjectLoaded { get; private set; }

		public string? ExportProject()
		{
			if (_project is not null)
			{
				return XamlServices.Save(_project);
			}
			return null;
		}

		public bool ImportProject(string import)
		{
			//TODO: Check exception handling if Config is broken..
			try
			{
				if (XamlServices.Parse(import) is not ProjectDefinition project)
				{
					_logger?.LogError("The project string could not be read in. It is broken! Check manually where the problem is.");
					return false;
				}
				_logger?.LogInformation("Am existing project from string will be created.");
				ImportProject(project);
				return true;
			}
			catch (Exception e)
			{
				_logger?.LogWarning(e, "Could not deserialize the project file.");
				return false;
			}
		}

		public void CreateNewProject()
		{
			_logger?.LogInformation("A new project will be created.");
			ImportProject(new ProjectDefinition());
		}

		public void ImportProject(ProjectDefinition import)
		{
			if (_project is not null)
			{
				UnObserveProject(_project);
			}
			_logger?.LogInformation("A project from object will be created.");
			_project = import;
			ObserveProject(_project);
			RaiseProjectLoaded();
			_logger?.LogInformation("The project was successfully imported.");
		}


		public LayerBaseDefinition? TryGetLayer(string id)
		{
			if (_project?.LayerGroups is IEnumerable<RenderTreeDefinition> renderTrees)
			{
				foreach (var renderTree in renderTrees)
				{
					var result = renderTree.TryGetLayer(id);
					if (result is not null)
					{
						return result;
					}
				}
			}
			_logger?.LogWarning("The layer with the id '{id}' could not be found.", id);
			return null;
		}

		public NodeDefinition? TryGetNode(string id)
		{
			var node = _project?.Nodes.FirstOrDefault(n => n.Id == id);
			if (node is null)
			{
				_logger?.LogWarning("The node with the id '{id}' could not be found.", id);
			}
			return node;
		}

		public LayerGroupDefinition? TryGetLayerGroup(string layerGroupId)
		{
			//TODO: Maybe Log if the node does not exist?
			var layerGroup = _project?.LayerGroups.FirstOrDefault(r => r.Id == layerGroupId);
			if (layerGroup is null)
			{
				_logger?.LogWarning("The renderTree with the id '{id}' could not be found.", layerGroupId);
			}
			return layerGroup;
		}

		public LayerGroupDefinition? TryGetLayerGroupForNode(string nodeId)
		{
			throw new NotImplementedException();
		}

		private void RaiseProjectLoaded()
		{
			IsProjectLoaded = true;
			_logger?.LogDebug("ProjectLoaded event will be raised.");
			ProjectLoaded?.Invoke(this, EventArgs.Empty);
		}

		private void ObserveProject(ProjectDefinition projectDefinition)
		{
			projectDefinition.ConfigurationChanged += Project_ConfigurationChanged;

			//TODO:Check CollectionChanges
			foreach (var layerGroup in projectDefinition.LayerGroups)
			{
				TraverseLayers(layerGroup.RenderTree.Layers, layer => layer.ConfigurationChanged += Project_ConfigurationChanged);
			}
		}



		private void UnObserveProject(ProjectDefinition projectDefinition)
		{
			projectDefinition.ConfigurationChanged -= Project_ConfigurationChanged;

			//Check CollectionChanges
			foreach (var renderTrees in projectDefinition.LayerGroups)
			{
				TraverseLayers(renderTrees.RenderTree.Layers, layer => layer.ConfigurationChanged -= Project_ConfigurationChanged);
			}
		}

		private void TraverseLayers(IEnumerable<LayerBaseDefinition> layers, Action<LayerBaseDefinition> manipulation)
		{
			foreach (var layer in layers)
			{
				manipulation(layer);
				if (layer is SplitLayerDefinition splittLayer)
				{
					foreach (var splitLayerList in splittLayer.Childs)
					{
						TraverseLayers(splitLayerList, manipulation);
					}
				}
			}
		}

		private void Project_ConfigurationChanged(object? sender, ConfigurationChangedEventArgs e)
		{

			_configurationsChangedChannel.Writer.TryWrite(e.Context);
		}


		public IAsyncEnumerable<ConfigurationChangedContext> GetConfigurationChangedAllAsync(CancellationToken cancellationToken = default)
			=> _configurationsChangedChannel.Reader.ReadAllAsync(cancellationToken);

		public IEnumerable<NodeDefinition> GetNodes()
		{
			return _project?.Nodes.ToArray() ?? Enumerable.Empty<NodeDefinition>();
		}

		public IEnumerable<LayerGroupDefinition> GetLayerGroups()
		{
			return _project?.LayerGroups.ToArray() ?? Enumerable.Empty<LayerGroupDefinition>();
		}

		public LayerGroupDefinition? TryAddLayerGroup()
		{
			if (_project is not null)
			{
				var newRenderTree = new LayerGroupDefinition();
				_project.LayerGroups.Add(newRenderTree);
				return newRenderTree;
			}

			return null;
		}

		public bool TryRemoveLayerGroup(string renderTreeId)
		{
			if (_project is null)
			{
				_logger.LogWarning("No project initialized, cannot delete render tree {renderTreeId}", renderTreeId);
				return false;
			}

			foreach (var renderTree in _project.LayerGroups)
			{
				if (renderTree.Id == renderTreeId)
				{
					return _project.LayerGroups.Remove(renderTree);
				}
			}

			return false;
		}

		public bool TryRemoveLayer(string layerId)
		{
			if (_project is null)
			{
				_logger.LogWarning("No project initialized, cannot delete layer {layerId}", layerId);
				return false;
			}
			
			foreach (var layerGroup in _project.LayerGroups)
			{
				foreach (var layer in layerGroup.RenderTree.Layers)
				{
					if (layer.Id == layerId)
					{
						return layerGroup.RenderTree.Layers.Remove(layer);
					}
				}
			}

			return false;
		}

		public bool TryAddNode(string id)
		{
			if (_project is not null)
			{
				if (!_project.Nodes.Any(n => n.Id == id))
				{
					_project.Nodes.Add(new NodeDefinition() { Id = id });
				}
			}
			return false;
		}
	}
}
