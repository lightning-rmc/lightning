using Lightning.Controller.Host.DTO;
using Lightning.Controller.Lifetime;
using Lightning.Controller.Lifetime.Nodes;
using Lightning.Controller.Projects;
using Lightning.Core.Definitions;
using Lightning.Core.Lifetime;
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
						 join state in states on definition.Id equals state.NodeId into temp
						 from node in temp.DefaultIfEmpty((NodeId: definition.Id, State: NodeState.Offline))
						 select new NodeDTO(
							 definition.Id,
							 definition.Name,
							 node.State.ToString(),
							 definition.FramesPerSecond,
							 new(definition.Resolution.Width, definition.Resolution.Height)
						);

			return result;
		}
	}
}
