using Lightning.Controller.Media;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class MediaController : ControllerBase
	{
		private readonly IMediaService _mediaService;
		private readonly ILogger _logger;

		public MediaController(IMediaService mediaService, ILogger<MediaController> logger)
		{
			_mediaService = mediaService;
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<Media.Media> GetStoredFiles(bool ignoreCache= false)
		{
			return _mediaService.GetFiles(ignoreCache);
		}

		[HttpPost("upload")]
		[DisableRequestSizeLimit]
		[RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
		public async Task<IActionResult> UploadFile(List<IFormFile> mediaFiles)
		{
			_logger.LogInformation($"File upload called with {mediaFiles.Count} files");
			var results = new Dictionary<string, UploadDetails>();
			foreach (var file in mediaFiles)
			{
				try
				{
					await _mediaService.SaveFileAsync(file);
					results.Add(file.FileName, new(true, null));
				}
				catch (Exception ex)
				{
					results.Add(file.FileName, new(false, ex.Message));
				}
			}

			return Ok(results);
		}
	}

	record UploadDetails(bool Success, string? ErrorMessage = null);
}
