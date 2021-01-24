using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public interface INodeLifetimeService
	{
		IAsyncEnumerable<NodeState> GetNodeStateStream(string nodeId);

		Task UpdateNodeStateAsync(NodeState state, string? nodeId = null);
	}
}
