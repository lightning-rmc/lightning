using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public abstract class CommandHandler<TCommandRequest, TCommandResponse> :
		ICommandNotifier<TCommandRequest, TCommandResponse>,
		ICommandReceiver<TCommandRequest, TCommandResponse>
	{
		public event EventHandler<CommandRequestEventArgs<TCommandRequest>>? CommandRequested;
		public event EventHandler<CommandResponseEventArgs<TCommandResponse>>? CommandResponded;

		private readonly Channel<TCommandResponse> _responseChannel;
		private readonly ILogger? _logger;

		public CommandHandler(ILogger? logger = null)
		{
			_responseChannel = Channel.CreateUnbounded<TCommandResponse>();
			_logger = logger;
		}

		public IAsyncEnumerable<TCommandResponse> GetCommandResponsesAllAsync()
			=> _responseChannel.Reader.ReadAllAsync();

		public async Task InvokeCommandRequestAsync(TCommandRequest request, CancellationToken token = default)
		{
			_logger?.LogInformation("Invocation was request to handle the command: {request}", request);
			var eventArgs = new CommandRequestEventArgs<TCommandRequest>(request, token: token);
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

			var response = ConvertRequestToResponse(request);
			_logger?.LogInformation("Handler Changed to Response: {response}", response);
			_ = Task.Run(() => CommandResponded?.Invoke(this, new CommandResponseEventArgs<TCommandResponse>(response)), CancellationToken.None);
			await _responseChannel.Writer.WriteAsync(response, token);
			_ = Task.Run(() => InvokeSubsequentRequest(response), CancellationToken.None);
		}

		protected abstract TCommandResponse ConvertRequestToResponse(TCommandRequest request);

		protected abstract Task InvokeSubsequentRequest(TCommandResponse response);
	}
}
