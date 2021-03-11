using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public interface INodeLifetimeNotifier
	{
		event EventHandler<CommandRequestEventArgs> CommandRequested;
		event EventHandler<CommandResponseEventArgs> CommandResponded;
	}


	public class CommandRequestEventArgs : EventArgs
	{
		private readonly List<Task> _tasks;
		public CommandRequestEventArgs(NodeCommandRequest request, CancellationToken token = default, string? nodeId = null)
		{
			NodeId = nodeId;
			_tasks = new List<Task>();
			Request = request;
			Token = token;
		}

		public string? NodeId { get; }
		public NodeCommandRequest Request { get; }
		public CancellationToken Token { get; }

		public void AddTask(Task task)
			=> _tasks.Add(task);

		public IReadOnlyCollection<Task> Tasks => _tasks;
	}

	public class CommandResponseEventArgs : EventArgs
	{

		public CommandResponseEventArgs(NodeCommandResponse response, string? nodeId = null)
		{
			NodeId = nodeId;
			Response = response;
		}

		public string? NodeId { get; }
		public NodeCommandResponse Response { get; }
	}
}
