using Lightning.Core.Lifetime;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public static class LifetimeServiceExtensions
	{
		public static Task GoLiveAsync(this INodeLifetimeService lifetimeService)
			=> lifetimeService.SetNodeCommandRequestAsync(NodeCommandRequest.GoLive, null);
		public static Task GoReadyAsync(this INodeLifetimeService lifetimeService)
			=> lifetimeService.SetNodeCommandRequestAsync(NodeCommandRequest.GoReady, null);
		public static Task ShowNodeInfoAsync(this INodeLifetimeService lifetimeService, string? nodeId = null)
			=> lifetimeService.SetNodeCommandRequestAsync(NodeCommandRequest.ShowInfo, nodeId);
		public static Task HideNodeInfoAsync(this INodeLifetimeService lifetimeService, string? nodeId = null)
			=> lifetimeService.SetNodeCommandRequestAsync(NodeCommandRequest.HideInfo, nodeId);
	}
}
