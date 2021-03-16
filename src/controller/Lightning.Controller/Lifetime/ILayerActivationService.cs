using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public interface ILayerActivationService
	{
		IEnumerable<(string LayerId, bool Active)> GetLayerActivationStates();

		IAsyncEnumerable<(string LayerId, bool Active)> GetLayerActivationsAllAsync(CancellationToken token = default);

		Task SetLayerActivationAsync(string layerId, bool active, CancellationToken token = default);
	}
}
