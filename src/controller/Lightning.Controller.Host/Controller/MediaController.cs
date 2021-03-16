using Lightning.Controller.Host.DTO;
using Lightning.Controller.Media;
using Lightning.Core.Definitions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace Lightning.Controller.Host.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class MediaController : ControllerBase
	{
		private readonly string MEDIA_ENDPOINT_PREFIX = "http://localhost:5000/media/";
		private readonly IMediaService _mediaService;
		private readonly ILogger _logger;

		public MediaController(IMediaService mediaService, ILogger<MediaController> logger)
		{
			_mediaService = mediaService;
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<MediaDTO> GetStoredFiles([FromQuery] bool useCache = true)
		{
			_logger.LogDebug("use cache: {}", useCache);
			var files = _mediaService.GetFiles(useCache);
			return files.Select(f => new MediaDTO
			{
				Name = f.Name,
				Extension = f.Extension,
				CreatedOn = f.CreatedOn,
				ModifiedOn = f.ModifiedOn,
				Size = f.Size,
				Hash = f.Hash,
				_self = new Uri(MEDIA_ENDPOINT_PREFIX + f.Name)
			});
		}

		[HttpDelete("{filename}")]
		public IActionResult DeleteMedia([FromRoute] string filename)
		{
			try
			{
				_mediaService.DeleteFile(filename);
				return Ok();
			}
			catch (FileNotFoundException)
			{
				return NotFound();
			}
		}

		[HttpPost("upload")]
		[DisableRequestSizeLimit]
		[RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
		public async Task<IActionResult> UploadFile(List<IFormFile> mediaFiles)
		{
			var results = new Dictionary<string, UploadDetails>();
			foreach (var file in mediaFiles)
			{
				try
				{
					await _mediaService.SaveFileAsync(file);
					results.Add(file.FileName, new(true, null));
					_logger.LogInformation("File uploaded: {}", file.FileName);
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
