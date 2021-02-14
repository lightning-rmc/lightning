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
		bool TryRemoveNode(string nodeId);

		//TODO: Check if needed? maybe not?
		IAsyncEnumerable<NodeCommandResponse> GetNodeCommandsAllAsync(string nodeId);
		IAsyncEnumerable<(string NodeId, NodeCommandResponse Command)> GetAllNodeCommandsAllAsync();

		Task SetNodeCommandRequestsAsync(NodeCommandRequest request, string? nodeId = null);

	}
}
