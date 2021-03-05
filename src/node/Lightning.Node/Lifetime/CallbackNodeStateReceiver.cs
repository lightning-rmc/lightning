using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node.Lifetime
{
	public class CallbackNodeStateReceiver : INodeStateReceiver
	{
		private readonly Func<NodeCommandRequest, Task> _callback;

		public CallbackNodeStateReceiver(Func<NodeCommandRequest, Task> callback)
		{
			_callback = callback;
		}

		public async Task NodeStateChangeRequestedAsync(NodeCommandRequest request)
		{
			await _callback(request);
		}
	}
}
