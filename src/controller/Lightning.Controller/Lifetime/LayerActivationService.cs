using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public class LayerActivationService : ILayerActivationService
	{
		private readonly ConcurrentDictionary<string, bool> _layerActivations;
		private readonly Channel<(string LayerId, bool Active)> _layerActivationUpdates;
		public LayerActivationService(ILogger<LayerActivationService>? logger = null)
		{
			//TODO: Handle state management?
			//		Layer change only in live possible

			_layerActivations = new ConcurrentDictionary<string, bool>();
			_layerActivationUpdates = Channel.CreateUnbounded<(string, bool)>();
			_ = Task.Run(async () =>
			{
				await foreach ((string LayerId, bool Active) in _layerActivationUpdates.Reader.ReadAllAsync())
				{
					_layerActivations.AddOrUpdate(LayerId, Active, (k, v) => Active);
				}
			}, CancellationToken.None); //TODO: end if the service will be disposed.
		}

		public IAsyncEnumerable<(string LayerId, bool Active)> GetLayerActivationsAllAsync(CancellationToken token = default)
		{
			return _layerActivationUpdates.Reader.ReadAllAsync(token);
		}

		//TODO: Check Concurrence
		public IEnumerable<(string LayerId, bool Active)> GetLayerActivationStates()
			=> _layerActivations.Select(kv => (kv.Key, kv.Value));

		public async Task SetLayerActivationAsync(string layerId, bool active, CancellationToken token = default)
			=> await _layerActivationUpdates.Writer.WriteAsync((layerId, active), token);
	}
}
