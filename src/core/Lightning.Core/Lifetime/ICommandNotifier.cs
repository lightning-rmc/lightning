using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public interface ICommandNotifier<TCommandRequest, TCommandResponse>
	{
		event EventHandler<CommandRequestEventArgs<TCommandRequest>> CommandRequested;
		event EventHandler<CommandResponseEventArgs<TCommandResponse>> CommandResponded;
	}
	public class CommandRequestEventArgs<TCommandRequest> : EventArgs
	{
		private readonly List<Task> _tasks;
		public CommandRequestEventArgs(TCommandRequest request, string? nodeId = null, CancellationToken token = default)
		{
			NodeId = nodeId;
			_tasks = new List<Task>();
			Request = request;
			Token = token;
		}

		public string? NodeId { get; }
		public TCommandRequest Request { get; }
		public CancellationToken Token { get; }

		public void AddTask(Task task)
			=> _tasks.Add(task);

		public IReadOnlyCollection<Task> Tasks => _tasks;
	}

	public class CommandResponseEventArgs<TCommandResponse> : EventArgs
	{

		public CommandResponseEventArgs(TCommandResponse response, string? nodeId = null)
		{
			NodeId = nodeId;
			Response = response;
		}

		public string? NodeId { get; }
		public TCommandResponse Response { get; }
	}
}
