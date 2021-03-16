using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Lightning.Node.Lifetime
{
	public class NodeCommandHandler : StateHandler<NodeState>, INodeStateNotifier, INodeStateReceiver
	{
		public NodeCommandHandler(ILogger<NodeCommandHandler>? logger = null) : base(logger)
		{

		}

		public NodeState State { get; }

		protected override async Task InvokeSubsequentStateRequest(NodeState response)
		{
			switch (response)
			{
				case NodeState.Start:
					await InvokeStateChangeAsync(NodeState.Connected);
					break;
				case NodeState.Connected:
					await InvokeStateChangeAsync(NodeState.Ready);
					break;
			}
		}
	}
}
