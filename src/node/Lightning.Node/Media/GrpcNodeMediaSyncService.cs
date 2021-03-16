using Grpc.Core;
using Lightning.Core.Generated;
using Lightning.Node.Communications;
using Lightning.Node.Lifetime;
using Microsoft.Extensions.Logging;

namespace Lightning.Node.Media
{
	internal class GrpcNodeMediaSyncService
	{
		private readonly INodeStateNotifier _nodeStateNotifier;
		private readonly IConnectionManager _connectionManager;
		private readonly IMediaSyncService _mediaSyncService;

		public GrpcNodeMediaSyncService(INodeStateNotifier nodeStateNotifier, IConnectionManager connectionManager, IMediaSyncService mediaSyncService, ILogger<GrpcNodeMediaSyncService>? logger = null)
		{
			_nodeStateNotifier = nodeStateNotifier;
			_connectionManager = connectionManager;
			_mediaSyncService = mediaSyncService;
			_nodeStateNotifier.StateChanged += async (s, e) =>
			{
				if (e.State == Core.Lifetime.NodeState.Connected)
				{
					var mediaClient = _connectionManager.GetMediaServiceClient();

					var result = mediaClient.GetMediaUpdates(new());

					await foreach (var update in result.ResponseStream.ReadAllAsync())
					{
						switch (update.UpdateType)
						{
							case UpdateType.Add:
							case UpdateType.Change:
								await _mediaSyncService.AddOrUpdateFileAsync(update.FileName);
								break;
							case UpdateType.Delete:
								_mediaSyncService.TryDeleteLocalFile(update.FileName);
								break;
						}
					}
				}
			};
		}
	}
}
