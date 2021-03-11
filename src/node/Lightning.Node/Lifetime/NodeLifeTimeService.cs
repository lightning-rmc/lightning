using Lightning.Core.Lifetime;
using Microsoft.Extensions.Logging;
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
		private readonly ILogger<NodeLifeTimeService>? _logger;

		public NodeLifeTimeService(ILogger<NodeLifeTimeService>? logger = null)
		{
			_responseChannel = Channel.CreateUnbounded<NodeCommandResponse>();
			_logger = logger;
		}

		public IAsyncEnumerable<NodeCommandResponse> GetNodeCommandResponsesAllAsync()
			=> _responseChannel.Reader.ReadAllAsync();

		public async Task InvokeCommandRequestAsync(NodeCommandRequest request, CancellationToken token = default)
		{
			_logger?.LogInformation("Node was request to handle the command: {request}", request);
			var eventArgs = new CommandRequestEventArgs(request, token);
			CommandRequested?.Invoke(this, eventArgs);

			if (eventArgs.Tasks.Count > 0)
			{
				_logger?.LogDebug("{count} services registered tasks for command request: {request}", eventArgs.Tasks.Count, request);
				await Task.WhenAll(eventArgs.Tasks);
			}
			else
			{
				_logger?.LogDebug("No Task was queued for command request '{request}', change directly to response.", request);
			}

			var response = NodeCommandHelper.ConvertRequestoResponse(request);
			_logger?.LogInformation("Node Changed to Response: {response}", response);
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
					case NodeCommandResponse.IsConnected:
					{
						await InvokeCommandRequestAsync(NodeCommandRequest.GoReady);
					}
					break;
				}
			});
		}
	}
}
