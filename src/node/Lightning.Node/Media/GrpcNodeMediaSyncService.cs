using Grpc.Core;
using Lightning.Core.Generated;
using Lightning.Core.Lifetime;
using Lightning.Core.Utils;
using Lightning.Node.Communications;
using Lightning.Node.Lifetime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Lightning.Node.Media
{
	internal class GrpcNodeMediaSyncService : ICreateOnStartup
	{
		private readonly ILogger? _logger;
		private readonly NodeConfiguration _options;
		private GrpcMediaSyncService.GrpcMediaSyncServiceClient _grpcClient = null!;
		private HttpClient _http = null!;

		public GrpcNodeMediaSyncService(IConnectionManager connectionManager,
			INodeStateNotifier nodeLifetime,
			IOptions<NodeConfiguration> options,
			ILogger<GrpcNodeMediaSyncService>? logger = null)
		{
			_grpcClient = connectionManager.GetMediaServiceClient();
			_http = connectionManager.GetHttpClient();
			_ = Task.Run(GetMediaSyncUpdatesAsync);
		}

		public async Task DownloadMediaAsync(string fileName)
		{
			var response = await _http.GetByteArrayAsync("/media/" + fileName);
			await File.WriteAllBytesAsync(Path.Combine(_options.Media.StoragePath, fileName), response);
		}

		public async Task SyncAllMediaAsync()
		{
			_logger?.LogInformation("Media syncing started");
			var media = await _http.GetFromJsonAsync<Core.Media.Media[]>("/api/media");
			var savedMedia = new DirectoryInfo(_options.Media.StoragePath).GetFiles();

			if (media is not null)
			{
				foreach (var m in media)
				{
					if (savedMedia.Any(f => f.Name == m.Name))
					{
						//TODO: Check null
						var file = savedMedia.FirstOrDefault(f => f.Name == m.Name);
						var sha256 = SHA256.Create();
						var localHash = sha256.ComputeHash(await File.ReadAllBytesAsync(file.FullName));
						if (BitConverter.ToString(localHash) != m.Hash)
						{
							_logger?.LogInformation("Downloading newer version for file: {}", m.Name);
							await DownloadMediaAsync(m.Name);
						}
						else
						{
							_logger?.LogInformation("Skipping file: {}", m.Name);
						}
					}
					else
					{
						_logger?.LogInformation("Downloading new file: {}", m.Name);
						await DownloadMediaAsync(m.Name);
					}
				}
			}
			_logger?.LogInformation("Media syncing finished");
		}

		private async Task GetMediaSyncUpdatesAsync()
		{
			var result = _grpcClient.GetMediaUpdates(new());

			await foreach (var update in result.ResponseStream.ReadAllAsync())
			{
				if (update.UpdateType == UpdateType.Delete)
				{
					File.Delete(Path.Combine(_options.Media.StoragePath, update.FileName));
					_logger?.LogInformation("Deleted file: {filename}", update.FileName);
				}
				else
				{
					await DownloadMediaAsync(update.FileName);
					_logger?.LogInformation("Created/Updated file: {filename}", update.FileName);
				}
			}
		}
	}
}
