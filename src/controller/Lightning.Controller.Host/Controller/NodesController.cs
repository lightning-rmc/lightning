using Lightning.Controller.Host.Controller.DTO;
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
		{
			var nodes = _nodeLifetimeSerivce.GetAllNodeStates();
			foreach (var node in nodes)
			{
				yield return new()
				{
					Id = node.NodeId,
					State = node.State
				};
			}
		}
	}
}
