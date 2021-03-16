using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Node.Lifetime
{
	public interface INodeStateNotifier : IStateNotifier<NodeState>
	{

	}
}
