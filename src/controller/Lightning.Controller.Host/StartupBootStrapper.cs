using Lightning.Controller.Lifetime;
using Lightning.Controller.Projects;
using Lightning.Core.Definitions;
using Lightning.Core.Definitions.Layers;
using Microsoft.Extensions.Hosting;
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
				RenderTrees = new()
				{
					new RenderTreeDefinition
					{
						Id = "node1",
						Layers = new()
						{
							new LayerDefinition
							{
								Id = "myLayer",
								Input = new FileInputLayerDefinition
								{
									Filename = "Alone_low.mp4"
								}
							},
							new WindowOutputDefinition()
						}
					}
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
