using Lightning.Controller.Projects;
using Lightning.Core.Definitions;
using Microsoft.AspNetCore.Mvc;
using Portable.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.Controller
{

	[Route("api/rendertrees")]
	[ApiController]
	public class RenderTreeController : ControllerBase
	{
		private readonly IProjectManager _projectManager;

		public RenderTreeController(IProjectManager projectManager)
		{
			_projectManager = projectManager;
		}

		[HttpGet]
		public IEnumerable<RenderTreeDefinition> GetRenderTrees()
		{
			var renderTrees = _projectManager.GetRenderTrees();
			return renderTrees;
		}

		[HttpGet("{renderTreeId}")]
		public IActionResult GetRenderTree([FromRoute] string renderTreeId)
		{
			var result = _projectManager.TryGetRenderTree(renderTreeId);
			if (result is not null)
			{
				return Ok(result);
			} else
			{
				return NotFound();
			}
		}

		[HttpGet("fornode/{nodeId}")]
		public string GetRenderTreeForNode([FromRoute]string nodeId)
		{
			var result = _projectManager.TryGetRenderTree(nodeId);
			return XamlServices.Save(result);
		}
	}
}
