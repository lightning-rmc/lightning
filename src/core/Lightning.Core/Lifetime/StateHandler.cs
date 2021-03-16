using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public abstract class StateHandler<TState> :
		IStateNotifier<TState>,
		IStateReceiver<TState>
	{
		public event EventHandler<StateChangeRequestEventArgs<TState>>? StateChangeRequested;
		public event EventHandler<StateChangedResponseEventArgs<TState>>? StateChanged;

		private readonly Channel<TState> _responseChannel;
		private readonly ILogger? _logger;

		public TState State { get; protected set; } = default!;

		public StateHandler(ILogger? logger = null)
		{
			_responseChannel = Channel.CreateUnbounded<TState>();
			_logger = logger;
			StateChanged += (s, e) => StateChangedCallback(e.State);
		}

		public IAsyncEnumerable<TState> GetStateResponsesAllAsync()
			=> _responseChannel.Reader.ReadAllAsync();

		public async Task InvokeStateChangeAsync(TState state, CancellationToken token = default)
		{
			_logger?.LogInformation("Invocation was request to handle the command: {request}", state);
			var eventArgs = new StateChangeRequestEventArgs<TState>(state, token: token);
			StateChangeRequested?.Invoke(this, eventArgs);

			if (eventArgs.Tasks.Count > 0)
			{
				_logger?.LogDebug("{count} services registered tasks for command request: {request}", eventArgs.Tasks.Count, state);
				await Task.WhenAll(eventArgs.Tasks);
			}
			else
			{
				_logger?.LogDebug("No Task was queued for command request '{request}', change directly to response.", state);
			}

			//TODO: add ErrorHandling
			_logger?.LogInformation("State changed to: {state}", state);
			State = state;
			_ = Task.Run(() => StateChanged?.Invoke(this, new StateChangedResponseEventArgs<TState>(state)), CancellationToken.None);
			await _responseChannel.Writer.WriteAsync(state, token);
		}

		protected abstract Task StateChangedCallback(TState state);
	}
}
