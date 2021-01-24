using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public interface INodeLifetimeService
	{

		//TODO: Change return type to ResponseObject?
		//TODO: Change argument to more useful data, e.g. max display size or fps
		bool AuthenticateNode(string nodeId); 

		IAsyncEnumerable<NodeState> GetNodeStatesAllAsync(string nodeId);

		Task UpdateNodeStateAsync(NodeState state, string? nodeId = null);

	}
}
