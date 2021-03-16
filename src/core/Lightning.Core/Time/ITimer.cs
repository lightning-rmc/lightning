using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Core.Time
{
	public interface ITimer
	{
		IAsyncEnumerable<int> GetTicksAllSync(CancellationToken token = default);
	}
}
