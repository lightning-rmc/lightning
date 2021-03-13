using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public enum NodeCommandResponse 
	{
		IsLive,
		IsReady,
		IsPreparing,
		IsConnected,
		ShowingInfo,
		HidingInfo,
		IsStarted,
		IsShutdown,
		HasError,
	}
}
