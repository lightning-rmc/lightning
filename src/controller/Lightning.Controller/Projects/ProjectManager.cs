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
				return false;
			}
			if (_project is not null)
			{
				UnObserveProject(_project);
			}
			//TODO: add Logging
			ObserveProject(project);
			_project = project;
			RaiseProjectLoaded();
			return true;
		}

		public void CreateNewProject()
		{
			if (_project is not null)
			{
				UnObserveProject(_project);
			}
			//TODO: add Logging
			_project = new();
			ObserveProject(_project);
			RaiseProjectLoaded();
		}

		public LayerBaseDefinition? TryGetLayer(string id)
		{
			if (_project?.RenderTrees is not IEnumerable<RenderTreeDefinition> renderTrees)
			{
				renderTrees = Enumerable.Empty<RenderTreeDefinition>();
			}
			foreach (var renderTree in renderTrees)
			{
				var result = renderTree.TryGetLayer(id);
				if (result is not null)
				{
					return result;
				}
			}
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
			=> _configurationsChangedChannel.Writer.TryWrite(e.Context);

		public IAsyncEnumerable<ConfigurationChangedContext> GetConfigurationChangedAllAsync(CancellationToken cancellationToken = default)
			=> _configurationsChangedChannel.Reader.ReadAllAsync(cancellationToken);
	}
}
