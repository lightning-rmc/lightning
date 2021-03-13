using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Lightning.Node.Lifetime
{
	public class NodeCommandHandler : CommandHandler<NodeCommandRequest, NodeCommandResponse>, INodeCommandNotifier, INodeCommandReceiver
	{
		public NodeCommandHandler(ILogger<NodeCommandHandler>? logger = null) : base(logger)
		{

		}

		protected override NodeCommandResponse ConvertRequestToResponse(NodeCommandRequest request)
			=> NodeCommandHelper.ConvertRequestoResponse(request);

		protected override async Task InvokeSubsequentRequest(NodeCommandResponse response)
		{
			switch (response)
			{
				case NodeCommandResponse.IsStarted:
					await InvokeCommandRequestAsync(NodeCommandRequest.TryConnecting);
					break;
				case NodeCommandResponse.IsConnected:
					await InvokeCommandRequestAsync(NodeCommandRequest.GoReady);
					break;
			}
		}
	}
}
