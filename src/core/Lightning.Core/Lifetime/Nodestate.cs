using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public enum NodeState
	{
		Start,
		Connected,
		Ready,
		Live,
		Preparing,
		Shutdown,
		Offline,
	}
}
