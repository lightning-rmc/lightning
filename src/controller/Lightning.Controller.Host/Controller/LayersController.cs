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
				if (_mediaService.ExistsMedia(request.Filename))
				{
					fileInputLayer.Filename = request.Filename;
					return Ok();
				}
				else
				{
					return BadRequest($"File '{request.Filename}' does not exist on the server");
				}
			}

			return BadRequest();
		}

		[HttpDelete("{layerId}")]
		public IActionResult DeleteLayer([FromRoute] string layerId)
		{
			var success = _projectManager.TryRemoveLayer(layerId);
			if (success)
			{
				return Ok();
			}
			else
			{
				return NotFound();
			}
		}

		public record SourceChangeRequest(string Filename);
	}
}
