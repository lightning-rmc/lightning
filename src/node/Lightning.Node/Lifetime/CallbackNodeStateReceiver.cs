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
		private readonly Func<NodeCommandRequest, Task<bool>> _callback;

		public CallbackNodeStateReceiver(Func<NodeCommandRequest, Task<bool>> callback)
		{
			_callback = callback;
		}

		public Task<bool> TryChangeNodeStateAsync(NodeCommandRequest request)
		{
			return _callback(request);
		}
	}
}
