using Lightning.Core.Utils;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Lightning.Controller.Media
{
	internal class MediaService : IMediaService, ICreateOnStartup
	{
		private readonly MediaSettings _settings;
		private readonly MD5 _md5;
		private readonly Dictionary<(string Name, DateTime LastWrite), string> _hashCache;
		private readonly FileSystemWatcher _watcher;
		private readonly ILogger _logger;

		public MediaService(IOptions<MediaSettings> mediaSettings, ILogger<MediaService> logger)
		{
			_logger = logger;
			_md5 = MD5.Create();
			_hashCache = new Dictionary<(string, DateTime), string>();
			_settings = mediaSettings.Value;
			_watcher = new FileSystemWatcher()
			{
				Path = _settings.StoragePath,
				EnableRaisingEvents = true
			};
			Directory.CreateDirectory(_settings.StoragePath);
			//_watcher.Created += OnFileChange;
			//_watcher.Changed += OnFileChange;
		}

		//private void OnFileChange(object sender, FileSystemEventArgs e)
		//{
		//	_logger.LogInformation($"Filesystem change: ${e.ChangeType} ${e.Name}");
		//	var hash = ComputeHash(e.FullPath);
		//	_hashCache.Add((e.Name!, DateTime.Now), hash);
		//}

		public IEnumerable<Media> GetFiles()
		{
			var dirInfo = new DirectoryInfo(_settings.StoragePath);

			foreach (var file in dirInfo.GetFiles())
			{
				var hashKey = (file.Name, file.LastWriteTime);
				if (!_hashCache.TryGetValue(hashKey, out var hash))
				{
					hash = ComputeHash(file.FullName);
					_hashCache.Add(hashKey, hash);
				}

				yield return new(file.Name, file.Extension, file.Length, file.CreationTime, file.LastWriteTime, hash.ToString());
			}
		}

		public async Task SaveFileAsync(IFormFile file)
		{
			var filePath = Path.Combine(_settings.StoragePath, file.FileName);
			using var stream = File.Create(filePath);
			await file.CopyToAsync(stream);
		}

		private string ComputeHash(string fullName)
		{
			using var stream = File.OpenRead(fullName);
			var hash = _md5.ComputeHash(stream);
			return BitConverter.ToString(hash);
		}
	}
}
