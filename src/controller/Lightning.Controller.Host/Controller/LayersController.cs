using Lightning.Controller.Host.DTO;
using Lightning.Controller.Projects;
using Lightning.Core.Definitions;
using Microsoft.AspNetCore.Mvc;

namespace Lightning.Controller.Host.Controller
{
	[Route("api/layers")]
	[ApiController]
	public class LayersController : ControllerBase
	{
		private readonly IProjectManager _projectManager;

		public LayersController(IProjectManager projectManager)
		{
			_projectManager = projectManager;
		}

		[HttpGet("{layerId}")]
		public ActionResult<LayerDTO> GetLayer([FromRoute] string layerId)
		{
			var layer = _projectManager.TryGetLayer(layerId);
			if (layer is null)
			{
				return NotFound();
			}

			if (layer is LayerDefinition ld)
			{
				return LayerDTO.FromDefinition(ld);
			}
			else
			{
				return BadRequest();
			}
		}
	}
}
