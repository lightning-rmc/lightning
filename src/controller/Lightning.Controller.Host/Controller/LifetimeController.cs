using Lightning.Controller.Lifetime;
using Microsoft.AspNetCore.Mvc;

namespace Lightning.Controller.Host.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class LifetimeController : ControllerBase
	{
		private readonly INodeLifetimeService _lifetimeService;

		public LifetimeController(INodeLifetimeService lifetimeService)
		{
			_lifetimeService = lifetimeService;
		}

		[HttpGet("register/{nodeId}")]
		public IActionResult RegisterNode([FromRoute] string nodeId)
		{
			var result = _lifetimeService.TryRegisterNode(nodeId);
			if (result)
			{
				return Ok();
			}
			else
			{
				return BadRequest(new { Message = "Node is already registered." });
			}
		}
	}
}
