using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime.Layers
{
	public interface ILiveHubServer
	{
		Task SetLayerActivation(string layerId, bool active);
	}
}
