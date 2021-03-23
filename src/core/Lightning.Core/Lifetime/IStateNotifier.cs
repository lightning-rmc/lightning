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
		private readonly List<(string Name, Task Task)> _tasks;
		public StateChangeRequestEventArgs(TState request, string? nodeId = null, CancellationToken token = default)
		{
			NodeId = nodeId;
			_tasks = new List<(string, Task)>();
			State = request;
			Token = token;
		}

		public string? NodeId { get; }
		public TState State { get; }
		public CancellationToken Token { get; }

		public void AddTask(string name, Task task)
			=> _tasks.Add((name, task));

		public IReadOnlyCollection<(string Name, Task Task)> Tasks => _tasks;
	}

	public class StateChangedResponseEventArgs<TState> : EventArgs
	{
		public StateChangedResponseEventArgs(TState response, string? nodeId = null)
		{
			NodeId = nodeId;
			State = response;
		}

		public string? NodeId { get; }
		public TState State { get; }
	}
}
