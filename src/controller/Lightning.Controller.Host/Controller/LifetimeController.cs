using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class LifetimeController : ControllerBase
	{
		public LifetimeController()
		{

		}


	}
}
