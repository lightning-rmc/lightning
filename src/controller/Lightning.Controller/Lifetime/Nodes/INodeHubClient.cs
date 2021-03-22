using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime.Nodes
{
	public interface INodeHubClient
	{
		Task NodeStateUpdateAsync(string id, string state, CancellationToken token = default);
		Task NodeConnectedUpdateAsync(string id, CancellationToken token = default);
	}
}
