using Lightning.Controller.Projects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.Controller
{
	[Route("api/project")]
	[ApiController]
	public class ProjectApiController : ControllerBase
	{
		private readonly IProjectLoader _projectLoader;

		public ProjectApiController(IProjectLoader projectLoader)
		{
			_projectLoader = projectLoader;
		}

		[HttpPost("save")]
		public async Task<IActionResult> SaveProject()
		{
			await _projectLoader.PersistProjectAsync();
			return Ok();
		}

		[HttpGet]
		public async Task ExportProject()
		{
			throw new NotImplementedException();
		}

		[HttpPost]
		public async Task ImportProject()
		{
			throw new NotImplementedException();
		}
	}
}
