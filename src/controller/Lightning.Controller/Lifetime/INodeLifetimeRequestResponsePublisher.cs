using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{

	internal interface INodeLifetimeRequestResponsePublisher
	{
		IAsyncEnumerable<NodeCommandRequest> GetNodeRequestsAllAsync(string nodeId);
		Task SetNodeResponseAsync(string nodeId, NodeCommandResponse nodeCommand);
	}
}
