using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime.Contracts
{
	public interface INodeHubClient
	{
		Task NodeStateUpdateAsync(string id, string state, CancellationToken token = default);
	}
}
