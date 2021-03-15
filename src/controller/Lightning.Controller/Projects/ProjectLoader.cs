using Lightning.Controller.Lifetime;
using Lightning.Core.Lifetime;
using Lightning.Core.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Controller.Projects
{
	public class ProjectLoader : ICreateOnStartup, IProjectLoader
	{
		private readonly ControllerSettings _config;
		private readonly IProjectManager _projectManager;
		private readonly ILogger<ProjectLoader>? _logger;

		public ProjectLoader(IControllerCommandNotifier commandNotifier,
			IProjectManager projectManager,
			IOptions<ControllerSettings> options,
			ILogger<ProjectLoader>? logger = null)
		{
			_config = options.Value;
			_projectManager = projectManager;
			_logger = logger;
			commandNotifier.CommandRequested
				+= CommandNotifier_CommandRequested;
		}

		private void CommandNotifier_CommandRequested(object? sender, CommandRequestEventArgs<ControllerCommandRequest> e)
		{
			if (e.Request == ControllerCommandRequest.OnStart)
			{
				e.AddTask(Task.Run(async () =>
				{
					var importError = true;
					var path = Path.Combine(Environment.CurrentDirectory, _config.ProjectPath);
					//TODO: Handle Exceptions
					if (File.Exists(_config.ProjectPath))
					{
						_logger.LogInformation("Try Load project from path: '{path}'", path);
						var content = await File.ReadAllTextAsync(_config.ProjectPath, e.Token);
						if (content is null)
						{
							_logger?.LogWarning("Project with path: '{path}' is empty");
						}
						else if (_projectManager.ImportProject(content))
						{
							_logger?.LogInformation("Successfully import project file from: '{path}'", path);
							importError = false;
						}
						else
						{
							_logger?.LogWarning("Project file is broken can't deserialize the project from path: '{path}'", path);
						}
					}

					if (importError)
					{
						_logger?.LogInformation("Create new project.");
						_projectManager.CreateNewProject();
					}
				}));
			}

			//if (e.Request == ControllerCommandRequest.OnShutdown)
			//{
			//	e.AddTask(Task.Run(async () =>
			//	{
			//		var path = Path.Combine(Environment.CurrentDirectory, _config.ProjectPath);
			//		var project = _projectManager.ExportProject();
			//		if (project is null)
			//		{
			//			_logger?.LogWarning("Could not serialize the project. project will not be saved.");
			//		}
			//		else
			//		{
			//			_logger?.LogInformation("Save project in file: '{path}'", path);
			//			//TODO: handle Exceptions
			//			await File.WriteAllTextAsync(_config.ProjectPath, project, e.Token);
			//		}
			//	}));
			//}
		}


		public async Task PersistProjectAsync(CancellationToken token = default)
		{
			var path = Path.Combine(Environment.CurrentDirectory, _config.ProjectPath);
			var project = _projectManager.ExportProject();
			if (project is null)
			{
				_logger?.LogWarning("Could not serialize the project. project will not be saved.");
			}
			else
			{
				_logger?.LogInformation("Save project in file: '{path}'", path);
				//TODO: handle Exceptions
				await File.WriteAllTextAsync(_config.ProjectPath, project, token);
			}
		}
	}
}
