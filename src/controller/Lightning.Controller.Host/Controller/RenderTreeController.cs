using Lightning.Controller.Projects;
using Microsoft.AspNetCore.Mvc;
using Portable.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.Controller
{

	[Route("api/[controller]")]
	[ApiController]
	public class RenderTreeController : ControllerBase
	{
		private readonly IProjectManager _projectManager;

		public RenderTreeController(IProjectManager projectManager)
		{
			_projectManager = projectManager;
		}

		[HttpGet("{nodeId}")]
		public string Index([FromRoute]string nodeId)
		{
			var result = _projectManager.GetRenderTree(nodeId);
			return XamlServices.Save(result);
		}
	}
}
