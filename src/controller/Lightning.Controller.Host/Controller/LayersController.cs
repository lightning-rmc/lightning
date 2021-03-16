using Lightning.Controller.Host.DTO;
using Lightning.Controller.Media;
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
		private readonly IMediaService _mediaService;

		public LayersController(IProjectManager projectManager, IMediaService mediaService)
		{
			_projectManager = projectManager;
			_mediaService = mediaService;
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

		[HttpPut("{layerId}/source")]
		public IActionResult SetLayerSource([FromRoute] string layerId, [FromBody] SourceChangeRequest request)
		{
			var layer = _projectManager.TryGetLayer(layerId);
			if (layer is null)
			{
				return NotFound(layerId);
			}

			if (layer is LayerDefinition { Input: FileInputLayerDefinition fileInputLayer })
			{
				if (_mediaService.ExistsMedia(request.filename))
				{
					fileInputLayer.Filename = request.filename;
					return Ok();
				} else
				{
					return BadRequest($"File '{request.filename}' does not exist on the server");
				}
			}

			return BadRequest();
		}


		public record SourceChangeRequest(string filename);
	}
}
