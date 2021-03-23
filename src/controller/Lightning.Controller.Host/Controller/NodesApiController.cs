using Lightning.Controller.Host.DTO;
using Lightning.Controller.Lifetime.Nodes;
using Lightning.Controller.Projects;
using Lightning.Core.Lifetime;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Lightning.Controller.Host.Controller
{
	[Route("api/nodes")]
	[ApiController]
	public class NodesApiController : ControllerBase
	{
		private readonly INodeLifetimeService _nodeLifetimeSerivce;
		private readonly IProjectManager _projectManager;

		public NodesApiController(INodeLifetimeService nodeLifetimeSerivce, IProjectManager projectManager)
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
						 select NodeDTO.FromDefinition(definition, node.State);

			return result;
		}


		[HttpGet("{nodeId}")]
		public ActionResult<NodeDTO> GetNodeById([FromRoute] string nodeId)
		{
			var node = _projectManager.TryGetNode(nodeId);
			if (node is null)
			{
				return NotFound();
			}

			var nodeState = _nodeLifetimeSerivce.GetAllNodeStates().FirstOrDefault(ns => ns.NodeId == node.Id);

			return NodeDTO.FromDefinition(node, nodeState.State);
		}


		[HttpPut("{nodeId}/name")]
		public ActionResult<NodeDTO> SetNodeName([FromRoute] string nodeId, [FromBody] NameSetDTO name)
		{
			var node = _projectManager.TryGetNode(nodeId);
			if (node is null)
			{
				return NotFound();
			}

			node.Name = name.Name;

			return NodeDTO.FromDefinition(node);
		}
	}

	public record NameSetDTO(string Name);
}
