using Lightning.Controller.Projects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lightning.Controller.Host.Controller
{
	[Route("api")]
	[ApiController]
	public class ProjectApiController : ControllerBase
	{
		private readonly IProjectLoader _projectLoader;
		private readonly IProjectManager _projectManager;
		private readonly ILogger<ProjectApiController>? _logger;

		public ProjectApiController(IProjectLoader projectLoader, IProjectManager projectManager, ILogger<ProjectApiController>? logger = null)
		{
			_projectLoader = projectLoader;
			_projectManager = projectManager;
			_logger = logger;
		}


		[HttpGet("project")]
		[HttpGet("project.json")]
		[Produces("application/json")]
		public IActionResult ExportProject()
		{
			var project = _projectManager.GetProject();
			if (project is not null)
			{
				return Ok(project);
			}
			else
			{
				return NotFound();
			}
		}

		[HttpGet("project.xml")]
		[Produces("application/xml")]
		public ContentResult ExportProjectAsXML()
		{
			var project = _projectLoader.ExportProjectToXAML();
			if (project is not null)
			{
				return new()
				{
					Content = project,
					ContentType = "application/xml",
					StatusCode = 200
				};
			}
			else
			{
				return new()
				{
					StatusCode = 404
				};
			}
		}
	}
}
