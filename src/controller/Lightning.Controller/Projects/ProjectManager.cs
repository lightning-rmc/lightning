using Lightning.Core.Configuration;
using Lightning.Core.Definitions;
using Lightning.Core.Definitions.Collections;
using Microsoft.Extensions.Logging;
using Portable.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

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
			if (XamlServices.Parse(import) is not ProjectDefinition project)
			{
				//TODO: add logging
				return false;
			}
			ImportProject(project);
			return true;
		}

		public void CreateNewProject()
			=> ImportProject(new ProjectDefinition());

		private void ImportProject(ProjectDefinition project)
		{
			if (_project is not null)
			{
				UnObserveProject(_project);
			}
			//TODO: add Logging
			_project = project;
			ObserveProject(_project);
			RaiseProjectLoaded();
		}

		public LayerBaseDefinition? TryGetLayer(string id)
		{
			if (_project?.RenderTrees is IEnumerable<RenderTreeDefinition> renderTrees)
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
			//TODO: add Logging
			return null;
		}

		public NodeDefinition? TryGetNode(string id)
		{
			//TODO: Maybe Log if the node does not exist?
			return _project?.Nodes.FirstOrDefault(n => n.Id == id);
		}

		public RenderTreeDefinition? TryGetRenderTree(string id)
		{
			//TODO: Maybe Log if the node does not exist?
			return _project?.RenderTrees.FirstOrDefault(r => r.Id == id);
		}

		private void RaiseProjectLoaded()
		{
			IsProjectLoaded = true;
			//TODO: add Logging
			ProjectLoaded?.Invoke(this, EventArgs.Empty);
		}

		private void ObserveProject(ProjectDefinition projectDefinition)
		{
			projectDefinition.ConfigurationChanged += Project_ConfigurationChanged;

			//Check CollectionChanges
			foreach (var renderTrees in projectDefinition.RenderTrees)
			{
				TraverseLayers(renderTrees.Layers, layer => layer.ConfigurationChanged += Project_ConfigurationChanged);
			}
			//projectDefinition.Nodes.CollectionChanged += NotifyIfCollectionElementChanged_EventCallback;
		}



		private void UnObserveProject(ProjectDefinition projectDefinition)
		{
			projectDefinition.ConfigurationChanged -= Project_ConfigurationChanged;

			//Check CollectionChanges
			foreach (var renderTrees in projectDefinition.RenderTrees)
			{
				TraverseLayers(renderTrees.Layers, layer => layer.ConfigurationChanged -= Project_ConfigurationChanged);
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
			//TODO: add logging
			//TODO: log if failed
			_configurationsChangedChannel.Writer.TryWrite(e.Context);
		}

<<<<<<< HEAD
		public IAsyncEnumerable<ConfigurationChangedContext> GetConfigurationChangedAllAsync(CancellationToken cancellationToken = default)
			=> _configurationsChangedChannel.Reader.ReadAllAsync(cancellationToken);
=======
		public IAsyncEnumerable<LayerPropertyUpdate> GetLayerPropertyUpdatesAllAsync(CancellationToken cancellationToken = default)
		{
			return _layerPropertyUpdates.Reader.ReadAllAsync(cancellationToken);
		}

		public IEnumerable<RenderTreeDefinition> GetRenderTreeDefinitions()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<LayerDefinition> GetLayers()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<NodeDefinition> GetNodes()
		{
			throw new NotImplementedException();
		}

		public RenderTreeDefinition GetRenderTree(string id)
		{
			throw new NotImplementedException();
		}

		public LayerDefinition GetLayer(string id)
		{
			throw new NotImplementedException();
		}

		public NodeDefinition GetNode(string id)
		{
			throw new NotImplementedException();
		}
>>>>>>> 86d4e994f08e96b91aea8ca72fde9393ba71e00f
	}
}
