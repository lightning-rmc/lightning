using Lightning.Core.Media;
using Lightning.Core.Utils;
using Lightning.Node.Communications;
using Lightning.Node.Lifetime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Node.Media
{
	class MediaSyncService : IMediaSyncService, ICreateOnStartup
	{
		private readonly NodeConfiguration _configuration;
		private readonly INodeStateNotifier _nodeStateNotifier;
		private readonly IConnectionManager _connectionManager;
		private readonly ILogger? _logger;
		private bool isSyncing = false;

		public MediaSyncService(INodeStateNotifier nodeStateNotifier, IConnectionManager connectionManager, IOptions<NodeConfiguration> options, ILogger<MediaSyncService>? logger = null)
		{
			_configuration = options.Value;
			_logger = logger;
			_nodeStateNotifier = nodeStateNotifier;
			_connectionManager = connectionManager;

			_nodeStateNotifier.StateChanged += OnNodeStateChanged;
		}

		private async void OnNodeStateChanged(object? sender, Core.Lifetime.StateChangedResponseEventArgs<Core.Lifetime.NodeState> e)
		{
			if (e.State == Core.Lifetime.NodeState.Connected)
			{
				var sw = Stopwatch.StartNew();
				_logger.LogInformation("Node connected to controller, starting media sync");
				await TrySyncMediaAsync();
				_logger.LogInformation("Media syncing finished after {elapsed}s", sw.Elapsed.TotalSeconds);
			}
		}

		public IEnumerable<Core.Media.Media> GetLocalFiles()
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

		public bool TryGetLocalFileByFilename(string filename, out Core.Media.Media? file)
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

		private async Task<bool> TrySyncMediaAsync(CancellationToken cancellationToken = default)
		{
			if (!isSyncing)
			{
				isSyncing = true;
				try
				{
					using var httpClient = _connectionManager.GetHttpClient();
					var remoteMedia = await httpClient.GetFromJsonAsync<Core.Media.Media[]>("/api/media", cancellationToken);
					var localMedia = GetLocalFiles();

					if (remoteMedia is not null)
					{
						// Delete media which is not on the server anymore
						foreach (var media in localMedia.Where(local => !remoteMedia.Any(remote => remote.Name == local.Name)))
						{
							TryDeleteLocalFile(Path.Combine(_configuration.Media.StoragePath, media.Name));
							_logger.LogDebug("Deleted local file {} because not present on remote", media.Name);
						}

						foreach (var media in remoteMedia)
						{
							if (!TryGetLocalFileByFilename(media.Name, out var file) || file?.Hash != media.Hash)
							{
								try
								{
									await DownloadMediaAsync(media.Name, cancellationToken);
									_logger.LogDebug("Downloaded File {filename}", media.Name);
								}
								catch (Exception e)
								{
									_logger.LogError("Could not download file {} from server: {}", media.Name, e);
								}
							}
						}
					}
					return true;
				}
				finally
				{
					isSyncing = false;
				}
			}
			else
			{
				_logger.LogWarning("Media sync is already running");
				return false;
			}
		}

		public async Task AddOrUpdateFileAsync(string filename, CancellationToken cancellationToken = default)
		{
			await DownloadMediaAsync(filename, cancellationToken);
		}

		public bool TryDeleteLocalFile(string name)
		{
			var path = Path.Combine(_configuration.Media.StoragePath, name);
			if (File.Exists(path))
			{
				File.Delete(path);
				return true;
			}

			return false;
		}

		private async Task DownloadMediaAsync(string fileName, CancellationToken cancellationToken = default)
		{
			using var httpClient = _connectionManager.GetHttpClient();
			var response = await httpClient.GetByteArrayAsync("/media/" + fileName, cancellationToken);
			await File.WriteAllBytesAsync(Path.Combine(_configuration.Media.StoragePath, fileName), response);
		}
	}
}
