using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{

	public interface INodeLifetimeRequestResponsePublisher
	{
		IAsyncEnumerable<NodeState> GetNodeRequestStatesAllAsync(string nodeId, CancellationToken token = default);
		Task SetNodeStateResponseAsync(string nodeId, NodeState nodeState, CancellationToken token = default);
	}
}
