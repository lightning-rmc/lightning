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

	[Route("api/rendertrees")]
	[ApiController]
	public class RenderTreeController : ControllerBase
	{
		private readonly IProjectManager _projectManager;

		public RenderTreeController(IProjectManager projectManager)
		{
			_projectManager = projectManager;
		}

		[HttpGet]
		public IEnumerable<RenderTreeDTO> GetRenderTrees()
		{
			var renderTrees = _projectManager.GetLayerGroups();
			foreach (var tree in renderTrees)
			{
				yield return new()
				{
					Id = tree.Id,
					Layers = TransformLayers(tree.RenderTree.Layers)
				};
			}
		}

		[HttpPost]
		public IActionResult AddRenderTree()
		{
			var newRenderTree = _projectManager.TryAddLayerGroup();
			if (newRenderTree is not null)
			{
				return Ok(newRenderTree);
			}

			throw new Exception("Could not create render tree");
		}

		[HttpGet("{renderTreeId}")]
		public ActionResult<RenderTreeDTO> GetRenderTree([FromRoute] string renderTreeId)
		{
			var result = _projectManager.TryGetLayerGroup(renderTreeId);
			if (result is not null)
			{
				return new RenderTreeDTO()
				{
					Id = result.Id,
					Layers = TransformLayers(result.RenderTree.Layers)
				};
			}
			else
			{
				return NotFound();
			}
		}

		[HttpDelete("{renderTreeId}")]
		public IActionResult DeleteRenderTree([FromRoute] string renderTreeId)
		{
			var success = _projectManager.TryRemoveRenderTree(renderTreeId);
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
		public string GetRenderTreeForNode([FromRoute] string nodeId)
		{
			var result = _projectManager.TryGetLayerGroup(nodeId);
			return XamlServices.Save(result);
		}


		[HttpPost("{renderTreeId}/layers")]
		public IActionResult AddLayerToRenderTree([FromRoute] string renderTreeId)
		{
			var renderTree = _projectManager.TryGetLayerGroup(renderTreeId);
			if (renderTree is null)
			{
				return BadRequest($"RenderTree with id {renderTreeId} does not exist");
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
