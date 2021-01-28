using Lightning.Core.Lifetime;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public static class LifetimeServiceExtensions
	{
		public static Task ConnectNodeAsync(this INodeLifetimeService lifetimeService, string nodeId)
			=> lifetimeService.UpdateNodeStateAsync(NodeState.Info, nodeId);

		public static Task DisconnectNodeAsync(this INodeLifetimeService lifetimeService, string nodeId)
			=> lifetimeService.UpdateNodeStateAsync(NodeState.Offline, nodeId);

		public static Task GoLiveAsync(this INodeLifetimeService lifetimeService)
			=> lifetimeService.UpdateNodeStateAsync(NodeState.Live, null);

		public static Task StartEditAsync(this INodeLifetimeService lifetimeService)
			=> lifetimeService.UpdateNodeStateAsync(NodeState.Edit, null);

		public static Task ShowNodeInfoAsync(this INodeLifetimeService lifetimeService, string? nodeId = null)
			=> lifetimeService.UpdateNodeStateAsync(NodeState.Info, nodeId);
	}
}
