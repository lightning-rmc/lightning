using Lightning.Controller.Host.DTO;
using Lightning.Controller.Lifetime;
using Lightning.Controller.Projects;
using Lightning.Core.Definitions;
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
		private readonly IProjectManager _projectManager;

		public NodesController(INodeLifetimeService nodeLifetimeSerivce, IProjectManager projectManager)
		{
			_nodeLifetimeSerivce = nodeLifetimeSerivce;
			_projectManager = projectManager;
		}


		[HttpGet]
		public IEnumerable<NodeDTO> GetNodes()
		{
			var states = _nodeLifetimeSerivce.GetAllNodeStates();
			var nodes = _projectManager.GetNodes();

			var result = from definition in nodes
						 join state in states on definition.Id equals state.NodeId
						 select new NodeDTO(
							 definition.Id,
							 definition.Name,
							 state.State,
							 definition.FramesPerSecond,
							 new(definition.Resolution.Width, definition.Resolution.Height)
						);

			return result;
		}
	}
}
