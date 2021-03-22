using Lightning.Controller.Host.DTO;
using Lightning.Controller.Projects;
using Lightning.Core.Definitions;
using Lightning.Core.Definitions.Collections;
using Microsoft.AspNetCore.Mvc;
using Portable.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lightning.Controller.Host.Controller
{

	[Route("api/layergroups")]
	[ApiController]
	public class LayerGroupsApiController : ControllerBase
	{
		private readonly IProjectManager _projectManager;

		public LayerGroupsApiController(IProjectManager projectManager)
		{
			_projectManager = projectManager;
		}

		[HttpGet]
		public IEnumerable<LayerGroupDTO> GetLayerGroups()
		{
			var layerGroups = _projectManager.GetLayerGroups();
			foreach (var group in layerGroups)
			{
				yield return new()
				{
					Id = group.Id,
					Layers = TransformLayers(group.RenderTree.Layers)
				};
			}
		}

		[HttpPost]
		public IActionResult AddLayerGroup()
		{
			var newLayerGroup = _projectManager.TryAddLayerGroup();
			if (newLayerGroup is not null)
			{
				return Ok(newLayerGroup);
			}

			throw new Exception("Could not create layer group");
		}

		[HttpGet("{layerGroupId}")]
		public ActionResult<LayerGroupDTO> GetLayerGroup([FromRoute] string layerGroupId)
		{
			var layerGroup = _projectManager.TryGetLayerGroup(layerGroupId);
			if (layerGroup is not null)
			{
				return new LayerGroupDTO()
				{
					Id = layerGroup.Id,
					Layers = TransformLayers(layerGroup.RenderTree.Layers)
				};
			}
			else
			{
				return NotFound();
			}
		}

		[HttpDelete("{layerGroupId}")]
		public IActionResult DeleteLayerGroup([FromRoute] string layerGroupId)
		{
			var success = _projectManager.TryRemoveLayerGroup(layerGroupId);
			if (success)
			{
				return Ok();
			}
			else
			{
				return NotFound();
			}
		}

		[HttpGet("fornode/{nodeId}")]
		public IActionResult GetLayerGroupForNode([FromRoute] string nodeId)
		{
			var result = _projectManager.TryGetLayerGroupForNode(nodeId);
			if (result is null)
			{
				return BadRequest();
			}
			//TODO: change to LayerGroup
			return new ObjectResult(XamlServices.Save(result.RenderTree));
		}


		[HttpPost("{layerGroupId}/layers")]
		public IActionResult AddLayerToLayerGroup([FromRoute] string layerGroupId)
		{
			var renderTree = _projectManager.TryGetLayerGroup(layerGroupId);
			if (renderTree is null)
			{
				return BadRequest($"LayerGroup with id {layerGroupId} does not exist");
			}

			var newLayer = new LayerDefinition()
			{
				Input = new FileInputLayerDefinition()
			};
			renderTree.RenderTree.Layers.Add(newLayer);
			return Created(newLayer.Id, TransformLayers(renderTree.RenderTree.Layers));
		}


		private IEnumerable<LayerDTO> TransformLayers(LayerBaseDefinitionCollection layers)
		{
			return layers
					.Where(l => l is LayerDefinition)
					.Cast<LayerDefinition>()
					.Select(l => LayerDTO.FromDefinition(l));
		}
	}
}
