using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.Hubs
{
	public interface ILiveHubClient
	{
		Task LayerActivationChanged(string layerId, bool isActive);
	}
}
