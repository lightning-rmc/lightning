using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public interface IStateNotifier<TState>
	{
		event EventHandler<StateChangeRequestEventArgs<TState>> StateChangeRequested;
		event EventHandler<StateChangedResponseEventArgs<TState>> StateChanged;
	}

	public class StateChangeRequestEventArgs<TState> : EventArgs
	{
		private readonly List<Task> _tasks;
		public StateChangeRequestEventArgs(TState request, string? nodeId = null, CancellationToken token = default)
		{
			NodeId = nodeId;
			_tasks = new List<Task>();
			Request = request;
			Token = token;
		}

		public string? NodeId { get; }
		public TState Request { get; }
		public CancellationToken Token { get; }

		public void AddTask(Task task)
			=> _tasks.Add(task);

		public IReadOnlyCollection<Task> Tasks => _tasks;
	}

	public class StateChangedResponseEventArgs<TState> : EventArgs
	{
		public StateChangedResponseEventArgs(TState response, string? nodeId = null)
		{
			NodeId = nodeId;
			Response = response;
		}

		public string? NodeId { get; }
		public TState Response { get; }
	}
}
