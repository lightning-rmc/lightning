using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public interface INodeCommandNotifier : ICommandNotifier<NodeCommandRequest, NodeCommandResponse>
	{

	}
}
