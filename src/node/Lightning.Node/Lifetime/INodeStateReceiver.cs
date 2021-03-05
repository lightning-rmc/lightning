using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node.Lifetime
{
	public interface INodeStateReceiver
	{
		Task<bool> TryChangeNodeStateAsync(NodeCommandRequest request);
	}
}
