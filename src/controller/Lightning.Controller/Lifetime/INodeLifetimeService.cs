using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public interface INodeLifetimeService
	{

		IEnumerable<(string NodeId, NodeState State)> GetAllNodeStates();

		//TODO: Change return type to ResponseObject?
		//TODO: Change argument to more useful data, e.g. max display size or fps
		bool TryRegisterNode(string nodeId);
		bool RemoveNode(string nodeId);

		IAsyncEnumerable<NodeState> GetNodeStatesAllAsync(string nodeId);
		IAsyncEnumerable<(string NodeId, NodeState State)> GetAllNodeStatesAllAsync();

		Task UpdateNodeStateAsync(NodeState state, string? nodeId = null);

	}
}
