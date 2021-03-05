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



		public static void RegisterCallback(this INodeStateManager nodeStateManager, Func<Task> callback, NodeCommandRequest request)
			=> nodeStateManager.RegisterCallback(async state =>
			{
				if (state == request)
				{
					await callback();
				}
			});

		public static void RegisterGoLiveCallback(this INodeStateManager nodeStateManager, Func<Task> callback)
			=> nodeStateManager.RegisterCallback(callback, NodeCommandRequest.GoLive);

		public static void RegisterGoReadyCallback(this INodeStateManager nodeStateManager, Func<Task> callback)
			=> nodeStateManager.RegisterCallback(callback, NodeCommandRequest.GoReady);
	}
}
