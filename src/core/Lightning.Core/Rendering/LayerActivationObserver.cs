using Grpc.Core;
using Lightning.Core.Generated;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering
{
	public class LayerActivationObserver<TFrame> : IDisposable
	{
		private bool _stopObserving = false;

		public LayerActivationObserver(IDictionary<string, ILayer<TFrame>> layers, GrpcLifetimeService.GrpcLifetimeServiceClient client)
		{

			_ = Task.Run(async () =>
			 {
				 var result = client.GetLayerActivationStream(new());
				 await foreach (var layerActivation in result.ResponseStream.ReadAllAsync())
				 {
					 if (layers.TryGetValue(layerActivation.LayerId, out var layer))
					 {
						 layer.IsActive = layerActivation.Active;
					 }
				 }
			 });
		}

		public void Dispose()
		{
			_stopObserving = true;
		}
	}
}
