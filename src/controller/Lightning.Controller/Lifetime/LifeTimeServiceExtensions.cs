using Lightning.Core.Lifetime;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public static class LifetimeServiceExtensions
	{
		public static Task GoLiveAsync(this INodeLifetimeService lifetimeService)
			=> lifetimeService.SetNodeCommandRequestsAsync(NodeCommandRequest.GoLive, null);
		public static Task GoReadyAsync(this INodeLifetimeService lifetimeService)
			=> lifetimeService.SetNodeCommandRequestsAsync(NodeCommandRequest.GoReady, null);
		public static Task ShowNodeInfoAsync(this INodeLifetimeService lifetimeService, string? nodeId = null)
			=> lifetimeService.SetNodeCommandRequestsAsync(NodeCommandRequest.ShowInfo, nodeId);
		public static Task HideNodeInfoAsync(this INodeLifetimeService lifetimeService, string? nodeId = null)
			=> lifetimeService.SetNodeCommandRequestsAsync(NodeCommandRequest.HideInfo, nodeId);
	}
}
