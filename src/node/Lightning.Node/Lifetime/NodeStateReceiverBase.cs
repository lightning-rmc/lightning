using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node.Lifetime
{
	public abstract class NodeStateReceiverBase : INodeStateReceiver
	{
		public virtual Task<bool> TryChangeNodeStateAsync(NodeCommandRequest request)
		{
			return request switch
			{
				NodeCommandRequest.GoLive => TryChangeToGoLiveAsync(),
				NodeCommandRequest.GoReady => TryChangeToGoReadyAsync(),
				NodeCommandRequest.TryConnecting => TryConnectingAsync(),
				_ => Task.FromResult(true)
			};
		}

		protected virtual Task<bool> TryChangeToGoReadyAsync()
		{
			return Task.FromResult(true);
		}

		protected virtual Task<bool> TryConnectingAsync()
		{
			return Task.FromResult(true);
		}

		protected virtual Task<bool> TryChangeToGoLiveAsync()
		{
			return Task.FromResult(true);
		}
	}
}
