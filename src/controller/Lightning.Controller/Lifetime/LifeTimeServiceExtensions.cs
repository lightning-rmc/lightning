using Lightning.Core.Lifetime;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public static class LifetimeServiceExtensions
	{
		public static Task GoLiveAsync(this INodeLifetimeService lifetimeService)
			=> lifetimeService.SetNodeCommandRequestAsync(NodeState.Live, null);
		public static Task GoReadyAsync(this INodeLifetimeService lifetimeService)
			=> lifetimeService.SetNodeCommandRequestAsync(NodeState.Ready, null);
		//public static Task ShowNodeInfoAsync(this INodeLifetimeService lifetimeService, string? nodeId = null)
		//	=> lifetimeService.SetNodeCommandRequestAsync(NodeState.ShowInfo, nodeId);
		//public static Task HideNodeInfoAsync(this INodeLifetimeService lifetimeService, string? nodeId = null)
		//	=> lifetimeService.SetNodeCommandRequestAsync(NodeState.HideInfo, nodeId);
	}
}
