using Lightning.Controller.Host.DTO;
using Lightning.Controller.Lifetime;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class NodesController : ControllerBase
	{
		private readonly INodeLifetimeService _nodeLifetimeSerivce;

		public NodesController(INodeLifetimeService nodeLifetimeSerivce)
		{
			_nodeLifetimeSerivce = nodeLifetimeSerivce;
		}


		[HttpGet]
		public IEnumerable<NodeDTO> GetNodes()
			=> _nodeLifetimeSerivce.GetAllNodeStates().Select(n => new NodeDTO(n.NodeId, "Node", n.State));

		[HttpPost("register/{nodeId}")]
		public IActionResult Register([FromRoute]string nodeId)
		{
			var result = _nodeLifetimeSerivce.TryRegisterNode(nodeId);
			if (result)
			{
				return Ok();
			}
			else
			{
				//TODO: Add useful error message
				//TODO: Add Logging
				return BadRequest("Node is already registered.");
			}
		}
	}
}
