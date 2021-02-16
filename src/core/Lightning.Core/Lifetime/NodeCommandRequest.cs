using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public enum NodeCommandRequest : byte
	{
		GoLive = 0,
		GoReady = 1,
		ShowInfo = 2,
		HideInfo = 3,
	}
}