using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Node.Lifetime
{
	public class NodeLifeTimeService : INodeLifetimeNotifier, INodeLifetimeReceiver
	{
		public event EventHandler<CommandRequestEventArgs>? CommandRequested;
		public event EventHandler<CommandResponseEventArgs>? CommandResponded;

		private readonly Channel<NodeCommandResponse> _responseChannel;

		public NodeLifeTimeService()
		{
			_responseChannel = Channel.CreateUnbounded<NodeCommandResponse>();
		}

		public IAsyncEnumerable<NodeCommandResponse> GetNodeCommandResponsesAllAsync()
			=> _responseChannel.Reader.ReadAllAsync();

		public async Task InvokeCommandRequestAsync(NodeCommandRequest request, CancellationToken token = default)
		{
			var eventArgs = new CommandRequestEventArgs(request, token);
			CommandRequested?.Invoke(this, eventArgs);

			if (eventArgs.Tasks.Count > 0)
			{
				await Task.WhenAll(eventArgs.Tasks);
			}

			var response = NodeCommandHelper.ConvertRequestoResponse(request);
			_ = Task.Run(() => CommandResponded?.Invoke(this, new CommandResponseEventArgs(response)));
			await _responseChannel.Writer.WriteAsync(response, token);

			_ = Task.Run(async () =>
			{
				switch (response)
				{
					case NodeCommandResponse.NodeIsStarted:
					{
						await InvokeCommandRequestAsync(NodeCommandRequest.TryConnecting);
					}
					break;
				}
			});
		}
	}
}
