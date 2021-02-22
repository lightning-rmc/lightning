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
		private readonly Channel<LayerPropertyUpdate> _layerPropertyUpdates;
		private ProjectDefinition? _project;

		public ProjectManager(ILogger<ProjectManager>? logger = null)
		{
			_logger = logger;
			_layerPropertyUpdates = Channel.CreateUnbounded<LayerPropertyUpdate>();
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
			projectDefinition.PropertyChanged += NotfiyPropertyChanged_EventCallback!;
			projectDefinition.RenderTrees.CollectionChanged += NotifyIfCollectionElementChanged_EventCallback;
			foreach (var renderTrees in projectDefinition.RenderTrees)
			{
				TraverseLayers(renderTrees.Layers, layer => layer.PropertyChanged += NotfiyPropertyChanged_EventCallback!);
			}
			projectDefinition.Nodes.CollectionChanged += NotifyIfCollectionElementChanged_EventCallback;
		}

		private void UnObserveProject(ProjectDefinition projectDefinition)
		{
			projectDefinition.PropertyChanged -= NotfiyPropertyChanged_EventCallback!;
			projectDefinition.RenderTrees.CollectionChanged -= NotifyIfCollectionElementChanged_EventCallback;
			foreach (var renderTrees in projectDefinition.RenderTrees)
			{
				TraverseLayers(renderTrees.Layers, layer => layer.PropertyChanged -= NotfiyPropertyChanged_EventCallback!);
			}
			projectDefinition.Nodes.CollectionChanged -= NotifyIfCollectionElementChanged_EventCallback;
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

		private void NotifyIfCollectionElementChanged_EventCallback(object? sender, NotifyCollectionChangedEventArgs e)
		{
			//TODO: Implement
			if (sender is null)
			{
				throw new ArgumentNullException(nameof(sender));
			}

			if (e is null)
			{
				throw new ArgumentNullException(nameof(e));
			}

			switch (sender)
			{
				default:
					break;
			}
		}

		private void NotfiyPropertyChanged_EventCallback(object? sender, PropertyChangedEventArgs eventArgs)
		{
			if (sender is null)
			{
				throw new ArgumentNullException(nameof(sender));
			}

			if (eventArgs is null)
			{
				throw new ArgumentNullException(nameof(eventArgs));
			}
			if (eventArgs.PropertyName is null)
			{
				//TODO: add Logging
				return;
			}

			switch (sender)
			{
				case LayerBaseDefinition layerDefinition:
				{
					//TODO: Handle sub routes like Transformation, color, etc...
					var property = layerDefinition.GetType().GetProperty(eventArgs.PropertyName);
					if (property is null)
					{
						//TODO: add Logging
						return;
					}
					//NOTE: we are sure the property has a value.
					var value = property.GetValue(layerDefinition)!;
					//TODO: We should avoid that every client gets all LayerPropertyChange notifications.
					//		Only those updates they are relevant should sent to client.
					//		For that we need the RenderTreeId, but we don't know that at this point.
					_layerPropertyUpdates.Writer.TryWrite(new(layerDefinition.Id, eventArgs.PropertyName, value));
				}
				break;

				default:
					break;
			}
		}

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
	}
}
