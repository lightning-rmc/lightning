using Lightning.Controller.Lifetime;
using Lightning.Controller.Projects;
using Lightning.Core.Definitions;
using Lightning.Core.Definitions.Layers;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Controller.Host
{
	public class StartupBootStrapper : IHostedService
	{
		private readonly INodeLifetimeService _nodeLifetimeService;
		private readonly IProjectManager _projectManager;

		public StartupBootStrapper(INodeLifetimeService nodeLifetimeService, IProjectManager projectManager)
		{
			_nodeLifetimeService = nodeLifetimeService;
			_projectManager = projectManager;
		}
		public Task StartAsync(CancellationToken cancellationToken)
		{
			_projectManager.ImportProject(new ProjectDefinition()
			{
				Nodes = new()
				{
					new NodeDefinition
					{
						Name = "Leinwand Rechts",
						FramesPerSecond = 60,
						Resolution = ResolutionDefinition.HDReady
					},
					new NodeDefinition
					{
						Name = "Leinwand Mitte",
						FramesPerSecond = 30,
						Resolution = ResolutionDefinition.UHD,
					},
					new NodeDefinition
					{
						Name = "Leinwand Backstage",
						FramesPerSecond = 30,
						Resolution = ResolutionDefinition.HDReady
					},
				},
				RenderTrees = new()
				{
					new RenderTreeDefinition
					{
						Layers = new()
						{
							new LayerDefinition
							{
								Input = new FileInputLayerDefinition
								{
									Filename = "Alone_low.mp4"
								}
							},
							new LayerDefinition
							{
								Input = new FileInputLayerDefinition()
								{
									Filename = "Test.mp4"
								}
							},
							new WindowOutputDefinition()
						}
					},
					new RenderTreeDefinition
					{
						Layers = new()
						{
							new LayerDefinition
							{
								Input = new FileInputLayerDefinition
								{
									Filename = "Alone_low.mp4"
								}
							},
							new LayerDefinition
							{
								Input = new FileInputLayerDefinition()
								{
									Filename = "Test.mp4"
								}
							},
							new LayerDefinition
							{
								Input = new FileInputLayerDefinition()
								{
									Filename = "Test.mp4"
								}
							},
							new LayerDefinition
							{
								Input = new FileInputLayerDefinition()
								{
									Filename = "Test.mp4"
								}
							},
						}
					},
					new RenderTreeDefinition
					{
						Layers = new()
						{
							new LayerDefinition
							{
								Input = new FileInputLayerDefinition
								{
									Filename = "Alone_low.mp4"
								}
							},
						}
					},
				}
			});

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}
