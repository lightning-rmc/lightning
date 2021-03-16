using Lightning.Core.Media;
using Lightning.Core.Utils;
using Lightning.Node.Communications;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Lightning.Node.Media
{
	class MediaSyncService : IMediaSyncService, ICreateOnStartup
	{
		private readonly NodeConfiguration _configuration;
		private readonly IHostLifetime _lifetime;
		private readonly IConnectionManager _connectionManager;
		private readonly ILogger? _logger;

		public MediaSyncService(IHostLifetime lifetime, IConnectionManager connectionManager, IOptions<NodeConfiguration> options, ILogger<MediaSyncService>? logger = null)
		{
			_configuration = options.Value;
			_logger = logger;
			_lifetime = lifetime;
			_connectionManager = connectionManager;
		}

		public IEnumerable<Core.Media.Media> GetLocalMedia()
		{
			var storedFiles = new DirectoryInfo(_configuration.Media.StoragePath).GetFiles();
			return storedFiles.Select(f => new Core.Media.Media()
			{
				Name = f.Name,
				Extension = f.Extension,
				CreatedOn = f.CreationTime,
				ModifiedOn = f.LastWriteTime,
				Hash = MediaUtil.ComputeHash(f.FullName),
				Size = f.Length
			});
		}

		public bool TryGetLocalMedia(string filename, out Core.Media.Media? file)
		{
			var path = Path.Combine(_configuration.Media.StoragePath, filename);
			if (File.Exists(path))
			{
				var fileInfo = new FileInfo(path);
				file = Core.Media.Media.FromFileInfo(fileInfo);
				return true;
			}
			else
			{
				file = null;
				return false;
			}
		}

		public async Task SyncMediaFromController()
		{
			using var httpClient = _connectionManager.GetHttpClient();
			_logger?.LogInformation("Media syncing started");
			var remoteMedia = await httpClient.GetFromJsonAsync<Core.Media.Media[]>("/api/media");
			var localMedia = GetLocalMedia();

			if (remoteMedia is not null)
			{
				// Delete media which is not on the server anymore
				foreach (var media in localMedia.Where(local => !remoteMedia.Any(remote => remote.Name == local.Name)))
				{
					TryDeleteLocalMedia(Path.Combine(_configuration.Media.StoragePath, media.Name));
					_logger.LogDebug("Deleted local file {} because not present on remote", media.Name);
				}

				foreach (var media in remoteMedia)
				{
					// Download file if not on node or hash does not match
					if (!TryGetLocalMedia(media.Name, out var file) || file?.Hash != media.Hash)
					{
						await DownloadMediaAsync(media.Name);
					}
				}
			}
		}

		private async Task DownloadMediaAsync(string fileName)
		{
			using var httpClient = _connectionManager.GetHttpClient();
			var response = await httpClient.GetByteArrayAsync("/media/" + fileName);
			await File.WriteAllBytesAsync(Path.Combine(_configuration.Media.StoragePath, fileName), response);
		}

		private bool TryDeleteLocalMedia(string name)
		{
			var path = Path.Combine(_configuration.Media.StoragePath, name);
			if (File.Exists(path))
			{
				File.Delete(path);
				return true;
			}

			return false;
		}
	}
}
