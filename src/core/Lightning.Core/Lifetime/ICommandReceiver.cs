using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public interface ICommandReceiver<TCommandRequest, TCommandResponse>
	{
		Task InvokeCommandRequestAsync(TCommandRequest request, CancellationToken token = default);

		IAsyncEnumerable<TCommandResponse> GetCommandResponsesAllAsync();
	}
}
