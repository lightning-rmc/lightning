using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public enum NodeCommandRequest
	{
		GoLive,
		GoReady,
		ShowInfo,
		HideInfo,
		TryConnecting,
		OnStart,
		OnShutdown,
	}
}
