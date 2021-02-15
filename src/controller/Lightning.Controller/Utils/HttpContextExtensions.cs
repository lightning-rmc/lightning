using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Utils
{
	public static class HttpContextExtensions
	{
		public static string? GetNodeId(this HttpContext httpContext)
		{
			if (httpContext.Request.Headers.TryGetValue("nodeId",out var nodeId))
			{
				//TODO: Maybe check typeconverting between StrinValues and string
				return nodeId;
			}
			return null;
		}
	}
}
