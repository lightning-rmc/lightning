using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public enum NodeCommandResponse 
	{
		IsLive = 0,
		IsReady = 1,
		IsPreparing = 2,
		IsConnected = 3,
		ShowingInfo = 4,
		HidingInfo = 5,
		NodeIsStarted = 6,
		NodeIsShutdown = 7,
		HasError = 255,
	}
}
