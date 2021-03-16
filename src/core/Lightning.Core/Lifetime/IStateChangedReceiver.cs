using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public interface IStateReceiver<TState>
	{
		TState State { get; }

		Task InvokeStateChangeAsync(TState request, CancellationToken token = default);

		IAsyncEnumerable<TState> GetStateResponsesAllAsync();
	}
}
