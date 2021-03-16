using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	//	public interface IApplicationFlagHandler<TFlag>
	//	{
	//		event EventHandler<ApplicationFlagChangeEventArgs<TFlag>> ApplicationFlagChanging;
	//		event EventHandler<ApplicationFlagChangedEventArgs<TFlag>> ApplicationFlagChanged;

	//		Task SetApplicationFlagAsync(ApplicationFlag<TFlag> applicationFlag, string? nodeid = null, CancellationToken token = default);

	//		IEnumerable<ApplicationFlag<TFlag>> GetApplicationFlags(string nodeId);

	//		IAsyncEnumerable<ApplicationFlag<TFlag>> GetApplicationFlagsAllAsync(CancellationToken token = default);
	//	}

	//	public record ApplicationFlag<TFlag>(TFlag Flagname, bool Flagvalue);

	//	public class ApplicationFlagChangeEventArgs<TFlag> : EventArgs
	//	{
	//		private readonly List<Task> _tasks;
	//		public ApplicationFlagChangeEventArgs(ApplicationFlag<TFlag> flag, string? nodeId, CancellationToken token)
	//		{
	//			Flagname = flag.Flagname;
	//			Flagvalue = flag.Flagvalue;
	//			NodeId = nodeId;
	//			Token = token;
	//			_tasks = new List<Task>();
	//		}

	//		public TFlag Flagname { get; }
	//		public bool Flagvalue { get; }
	//		public string? NodeId { get; }
	//		public CancellationToken Token { get; }

	//		public void AddTask(Task task)
	//			=> _tasks.Add(task);
	//		public IReadOnlyCollection<Task> Tasks => _tasks;
	//	}

	//	public class ApplicationFlagChangedEventArgs<TFlag> : EventArgs
	//	{
	//		public ApplicationFlagChangedEventArgs(ApplicationFlag<TFlag> flag, string? nodeId)
	//		{
	//			Flagname = flag.Flagname;
	//			Flagvalue = flag.Flagvalue;
	//			NodeId = nodeId;
	//		}

	//		public TFlag Flagname { get; }
	//		public bool Flagvalue { get; }
	//		public string? NodeId { get; }
	//	}
}
