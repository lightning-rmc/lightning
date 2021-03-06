using Lightning.Controller.Lifetime.Controller;
using Lightning.Core.Definitions;
using Lightning.Core.Lifetime;
using Lightning.Core.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portable.Xaml;
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

		public ProjectLoader(IControllerStateNotifier commandNotifier,
			IProjectManager projectManager,
			IOptions<ControllerSettings> options,
			ILogger<ProjectLoader>? logger = null)
		{
			_config = options.Value;
			_projectManager = projectManager;
			_logger = logger;
			commandNotifier.StateChangeRequested
				+= CommandNotifier_CommandRequested;
		}



		private void CommandNotifier_CommandRequested(object? sender, StateChangeRequestEventArgs<ControllerState> e)
		{
			if (e.State == ControllerState.Start)
			{
				e.AddTask("Load Project",Task.Run(async () =>
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
						else if (ImportProjectFromXAML(content))
						{
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

			//Autosave on shutdown
			//TODO:Change to flag
			//if (e.State == ControllerState.Shutdown)
			//{
			//	e.AddTask(Task.Run(async () =>
			//	{
			//		await PersistProjectAsync(e.Token);
			//	}, e.Token));
			//}
		}


		public async Task PersistProjectAsync(CancellationToken token = default)
		{
			var path = Path.Combine(Environment.CurrentDirectory, _config.ProjectPath);
			var project = ExportProjectToXAML();
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

		public string? ExportProjectToXAML()
		{
			var project = _projectManager.GetProject();
			if (project is not null)
			{
				return XamlServices.Save(project);
			}
			return null;
		}

		public bool ImportProjectFromXAML(string projectDefinition)
		{
			//TODO: Check exception handling if Config is broken..
			try
			{
				if (XamlServices.Parse(projectDefinition) is not ProjectDefinition project)
				{
					_logger?.LogError("Error in project file definition, could not parse project");
					return false;
				}
				_logger?.LogDebug("Project import STARTED");
				_projectManager.ImportProject(project);
				_logger?.LogDebug("Project import FINISHED");
				return true;
			}
			catch (Exception e)
			{
				_logger?.LogWarning(e, "Could not deserialize the project file.");
				return false;
			}
		}
	}
}
