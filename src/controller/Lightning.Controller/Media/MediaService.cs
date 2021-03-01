using Lightning.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Controller.Media
{
	internal class MediaService : IMediaService, ICreateOnStartup
	{
		private readonly MediaSettings _settings;
		private readonly ConcurrentDictionary<string, string> _hashCache;
		private readonly FileSystemWatcher _watcher;
		private readonly ILogger _logger;
		private readonly Channel<(string fileName, UpdateType updateType)> _updates;

		public MediaService(IOptions<MediaSettings> mediaSettings, ILogger<MediaService> logger)
		{
			_logger = logger;
			_hashCache = new ConcurrentDictionary<string, string>();
			_settings = mediaSettings.Value;
			_updates = Channel.CreateUnbounded<(string, UpdateType)>();
			Directory.CreateDirectory(_settings.StoragePath);
			//_watcher = new FileSystemWatcher()
			//{
			//	Path = _settings.StoragePath,
			//	EnableRaisingEvents = true
			//};
		}


		public IEnumerable<Core.Media.Media> GetFiles(bool ignoreCache = false)
		{
			var dirInfo = new DirectoryInfo(_settings.StoragePath);

			foreach (var file in dirInfo.GetFiles())
			{
				var hash = _hashCache.GetOrAdd(file.Name, k => ComputeHash(file.FullName));

				var link = $"http://localhost:5000/media/{file.Name}";

				yield return new(
					file.Name,
					file.Extension,
					file.Length,
					file.CreationTime,
					file.LastWriteTime,
					hash,
					new Uri(link)
				);
			}
		}

		public async Task SaveFileAsync(IFormFile file)
		{
			var filePath = Path.Combine(_settings.StoragePath, file.FileName);
			using var stream = File.Create(filePath);
			await file.CopyToAsync(stream);
			await _updates.Writer.WriteAsync((file.FileName, UpdateType.ADDED));
		}


		public IAsyncEnumerable<(string fileName, UpdateType updateType)> GetUpdatesAllAsync()
			=> _updates.Reader.ReadAllAsync();


		private string ComputeHash(string fullName)
		{
			using var algorithm = SHA256.Create();
			using var stream = File.OpenRead(fullName);
			var hash = algorithm.ComputeHash(stream);
			return BitConverter.ToString(hash);
		}
	}
}
