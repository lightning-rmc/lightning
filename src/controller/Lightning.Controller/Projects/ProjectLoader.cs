using Lightning.Controller.Lifetime;
using Lightning.Core.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace Lightning.Controller.Projects
{
	public class ProjectLoader : ICreateOnStartup
	{

		public ProjectLoader(IControllerCommandNotifier commandNotifier,
			IProjectManager projectManager,
			IOptions<ControllerSettings> options,
			ILogger<ProjectLoader>? logger = null)
		{
			var config = options.Value;
			commandNotifier.CommandRequested += (s, e) =>
			{
				if (e.Request == ControllerCommandRequest.OnStart)
				{
					e.AddTask(Task.Run(async () =>
					{
						if (File.Exists(config.ProjectPath))
						{
							//TODO: log message
							var content = await File.ReadAllTextAsync(config.ProjectPath, e.Token);
							if (content is null)
							{
								//TODO: log message
								return;
							}
							if (projectManager.ImportProject(content))
							{
								//TODO: log message
							}
						}
						else
						{
							//TODO: log message
							projectManager.CreateNewProject();
						}
					}));
				}

				if (e.Request == ControllerCommandRequest.OnShutdown)
				{
					e.AddTask(Task.Run(async () =>
					{
						var project = projectManager.ExportProject();
						if (project is null)
						{
							//TODO: add log message
							return;
						}

						//TODO: add log message
						await File.WriteAllTextAsync(config.ProjectPath, project, e.Token);
					}));
				}
			};
		}
	}
}
