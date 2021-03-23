using System.Collections.Generic;
using System.Threading;

namespace Lightning.Controller.Timer
{
	public interface ITimerService
	{
		bool IsActive { get; }
		IAsyncEnumerable<int> GetTimerTicks(CancellationToken token = default);
	}
}
