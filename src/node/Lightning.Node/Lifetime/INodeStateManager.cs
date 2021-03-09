using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Node.Lifetime
{
	public interface INodeStateManager
	{
		void RegisterCallback(Func<NodeCommandRequest, Task<bool>> callback);

		Task ReportNodeCommandRequestAsync(NodeCommandRequest request);

		IAsyncEnumerable<NodeCommandResponse> GetNodeCommandResponseAllAsync(CancellationToken token = default);
	}
}
