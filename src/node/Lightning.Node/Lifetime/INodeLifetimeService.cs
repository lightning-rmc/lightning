using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node.Lifetime
{
	public interface INodeLifetimeService
	{
		Task SetCommandResponseAsync(NodeCommandResponse command);
		IAsyncEnumerable<NodeCommandRequest> GetCommandRequestsAllAsync();
	}
}
