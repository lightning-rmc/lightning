using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public static class LifeTimeServiceExtensions
	{
		public static Task ConnectNodeAsync(this INodeLifetimeService lifetimeService, string nodeId)
			=> lifetimeService.UpdateNodeStateAsync(NodeState.Info, nodeId);

		public static Task DisconnectNodeAsync(this INodeLifetimeService lifetimeService, string nodeId)
			=> lifetimeService.UpdateNodeStateAsync(NodeState.Offline, nodeId);

		public static Task GoLiveAsync(this INodeLifetimeService lifetimeService, string nodeId)
			=> lifetimeService.UpdateNodeStateAsync(NodeState.Live, nodeId);

		public static Task StartEditAsync(this INodeLifetimeService lifetimeService, string nodeId)
			=> lifetimeService.UpdateNodeStateAsync(NodeState.Edit, nodeId);

		public static Task ShowNodeInfoAsync(this INodeLifetimeService lifetimeService, string nodeId)
			=> lifetimeService.UpdateNodeStateAsync(NodeState.Info, nodeId);
	}
}
