using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Node.Lifetime
{
	public interface INodeStateReceiver : IStateReceiver<NodeState>
	{
		NodeState State { get; }
	}
}
