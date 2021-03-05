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
				NodeCommandRequest.GoLive => TyChangeToGoLiveAsync(),
				NodeCommandRequest.GoReady => TryChangeToGoReadyAsync(),
				_ => Task.FromResult(true)
			};
		}

		protected virtual Task<bool> TryChangeToGoReadyAsync()
		{
			return Task.FromResult(true);
		}

		protected virtual Task<bool> TyChangeToGoLiveAsync()
		{
			return Task.FromResult(true);
		}
	}
}
