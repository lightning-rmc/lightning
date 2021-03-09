using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node.Lifetime
{
	public static class NodeStateManagementExtensions
	{



		public static void RegisterCallback(this INodeStateManager nodeStateManager, Func<Task<bool>> callback, NodeCommandRequest request)
			=> nodeStateManager.RegisterCallback(async state =>
			{
				if (state == request)
				{
					return await callback();
				} else
				{
					return true;
				}
			});

		public static void RegisterGoLiveCallback(this INodeStateManager nodeStateManager, Func<Task<bool>> callback)
			=> nodeStateManager.RegisterCallback(callback, NodeCommandRequest.GoLive);

		public static void RegisterGoReadyCallback(this INodeStateManager nodeStateManager, Func<Task<bool>> callback)
			=> nodeStateManager.RegisterCallback(callback, NodeCommandRequest.GoReady);
	}
}
