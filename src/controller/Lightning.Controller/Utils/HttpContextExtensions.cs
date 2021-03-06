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
		//TODO: Add description
		public static string? TryGetNodeId(this HttpContext httpContext)
		{
			if (httpContext.Request.Headers.TryGetValue("nodeId",out var nodeId))
			{
				//TODO: Maybe check type converting between StrinValues and string
				return nodeId;
			}
			return null;
		}

		//TODO: Add description
		public static string GetNodeId(this HttpContext httpContext)
		{
			var result = TryGetNodeId(httpContext);
			if (result is null)
			{
				throw new InvalidOperationException("Their is no NodeId set in the Request Headers. " +
					"Check if the client sends the nodeId in the Header with key: 'nodeId'!");
			}
			return result;
		}
	}
}
