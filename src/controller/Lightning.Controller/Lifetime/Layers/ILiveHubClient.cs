using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime.Layers
{
	public interface ILiveHubClient
	{
		Task LayerActivationChangedAsync(string layerId, bool isActive);
	}
}
