using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime.Controller
{
	public enum ControllerCommandResponse
	{
		OnStarted,
		OnShutdown,
		IsReady,
		IsLive
	}
}
