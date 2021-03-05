using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Channels;

namespace Lightning.Node.Lifetime
{
	public class NodeStateManager : INodeStateManager
	{
		private readonly ConcurrentBag<INodeStateReceiver> _stateReceivers;
		private readonly Channel<NodeCommandResponse> _commandResponse;

		public NodeStateManager(IEnumerable<INodeStateReceiver> stateReceivers)
		{
			_stateReceivers = new ConcurrentBag<INodeStateReceiver>(stateReceivers);
			_commandResponse = Channel.CreateUnbounded<NodeCommandResponse>();
		}

		public IAsyncEnumerable<NodeCommandResponse> GetNodeCommandResponseAllAsync(CancellationToken token = default)
			=> _commandResponse.Reader.ReadAllAsync(token);

		public void RegisterCallback(Func<NodeCommandRequest, Task> callback)
		{
			_stateReceivers.Add(new CallbackNodeStateReceiver(callback));
		}

		public async Task ReportNodeCommandRequestAsync(NodeCommandRequest request)
		{
			var result = true;
			foreach (var receiver in _stateReceivers.ToArray())
			{
				result = await receiver.TryChangeNodeStateAsync(request);
				if (!result)
				{
					break;
				}
			}

			if (result)
			{
				await HandleResponseAsync(request);
			}
			else
			{
				//TODO: add logging
				//TODO: report it to the controller
			}
		}

		private async Task HandleResponseAsync(NodeCommandRequest request)
		{
			var response = request switch
			{
				NodeCommandRequest.GoLive => NodeCommandResponse.IsLive,
				NodeCommandRequest.GoReady => NodeCommandResponse.IsReady,
				NodeCommandRequest.HideInfo => NodeCommandResponse.HidingInfo,
				NodeCommandRequest.ShowInfo => NodeCommandResponse.ShowingInfo,
				_ => NodeCommandResponse.HasError
			};
			await _commandResponse.Writer.WriteAsync(response);
		}
	}
}
