using Lightning.Core.Media;
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
		private readonly ILogger _logger;
		private readonly Channel<(string fileName, UpdateType updateType)> _updates;

		public MediaService(IOptions<MediaSettings> mediaSettings, ILogger<MediaService> logger)
		{
			_logger = logger;
			_hashCache = new ConcurrentDictionary<string, string>();
			_settings = mediaSettings.Value;
			_updates = Channel.CreateUnbounded<(string, UpdateType)>();
			Directory.CreateDirectory(_settings.StoragePath);
		}


		public IEnumerable<Core.Media.Media> GetFiles(bool useCache = true)
		{
			var dirInfo = new DirectoryInfo(_settings.StoragePath);

			foreach (var file in dirInfo.GetFiles())
			{
				string hash;
				
				if (useCache)
				{
					hash = _hashCache.GetOrAdd(file.Name, k => MediaUtil.ComputeHash(file.FullName));
				} else
				{
					hash = MediaUtil.ComputeHash(file.FullName);
					_hashCache.TryAdd(file.FullName, hash);
				}

				yield return new Core.Media.Media()
				{
					Name = file.Name,
					Extension = file.Extension,
					CreatedOn = file.CreationTime,
					ModifiedOn = file.LastWriteTime,
					Hash = hash,
					Size = file.Length
				};
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


		public void DeleteFile(string filename)
		{
			var path = Path.Combine(_settings.StoragePath, filename);
			_logger.LogDebug("path: {}", path);
			File.Delete(path);
		}

		public bool ExistsMedia(string filename)
		{
			var path = Path.Combine(_settings.StoragePath, filename);
			return File.Exists(path);
		}
	}
}
