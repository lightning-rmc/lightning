using System.Threading.Tasks;

namespace Lightning.Controller.Host.Hubs
{
	public interface INodeHubClient
	{
		Task NodeStateUpdated(string nodeId, string nodeState);
		Task NodeConnected(string nodeId);
	}
}
