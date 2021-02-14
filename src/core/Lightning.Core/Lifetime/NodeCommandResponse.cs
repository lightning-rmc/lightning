using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public enum NodeCommandResponse : byte
	{
		IsLive = 0,
		IsReady = 1,
		IsPreparing = 2,
		ShowingInfo = 3,
		HidingInfo = 4,
		HasError = 255,
	}
}
