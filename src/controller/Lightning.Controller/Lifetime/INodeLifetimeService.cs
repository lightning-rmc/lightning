using Lightning.Core.Lifetime;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public interface INodeLifetimeService
	{

		IEnumerable<(string NodeId, NodeState State)> GetAllNodeStates();

		//TODO: Change return type to ResponseObject?
		//TODO: Change argument to more useful data, e.g. max display size or fps
		void TryRegisterNode(string nodeId);
		bool TryRemoveNode(string nodeId);

		//TODO: Check if needed? maybe not?
		IAsyncEnumerable<NodeState> GetNodeStatsAllAsync(string nodeId, CancellationToken token = default);
		IAsyncEnumerable<NodeStateUpdate> GetAllNodeStatesAllAsync(CancellationToken token = default);

		Task SetNodeCommandRequestAsync(NodeState request, string? nodeId = null, CancellationToken token = default);

	}

	public record NodeStateUpdate(string NodeId, NodeState State);
}
