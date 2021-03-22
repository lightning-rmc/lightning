using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
	public class LayerActivationService : ILayerActivationService
	{
		private readonly ConcurrentDictionary<string, bool> _layerActivations;
		private readonly ConcurrentDictionary<Channel<(string LayerId, bool Active)>, object?> _layerActivationUpdates;
		public LayerActivationService(ILogger<LayerActivationService>? logger = null)
		{
			//TODO: Handle state management?
			//		Layer change only in live possible

			_layerActivations = new ConcurrentDictionary<string, bool>();

			_layerActivationUpdates = new ConcurrentDictionary<Channel<(string LayerId, bool Active)>, object?>();
			_ = Task.Run(async () =>
			{
				await foreach ((string LayerId, bool Active) in GetLayerActivationsAllAsync())
				{
					_layerActivations.AddOrUpdate(LayerId, Active, (k, v) => Active);
				}
			}, CancellationToken.None); //TODO: end if the service will be disposed.
		}

		public async IAsyncEnumerable<(string LayerId, bool IsActive)> GetLayerActivationsAllAsync([EnumeratorCancellation]CancellationToken token = default)
		{
			var channel = Channel.CreateUnbounded<(string, bool)>();
			_layerActivationUpdates.TryAdd(channel, null);
			try
			{
				await foreach (var item in channel.Reader.ReadAllAsync(token))
				{
					yield return item;
				}
			}
			finally
			{
				channel.Writer.TryComplete();
				_layerActivationUpdates.TryRemove(channel, out _);
			}
		}

		//TODO: Check Concurrence
		public IEnumerable<(string LayerId, bool IsActive)> GetLayerActivationStates()
			=> _layerActivations.Select(kv => (kv.Key, kv.Value));

		public async Task SetLayerActivationAsync(string layerId, bool active, CancellationToken token = default)
		{
			foreach (var channel in _layerActivationUpdates.Keys)
			{
				await channel.Writer.WriteAsync((layerId, active), token);
			}
		}
	}
}
