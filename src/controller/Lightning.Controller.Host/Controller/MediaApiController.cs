using Lightning.Controller.Host.DTO;
using Lightning.Controller.Media;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.Controller
{
	[Route("api/media")]
	[ApiController]
	public class MediaApiController : ControllerBase
	{
		private readonly IMediaService _mediaService;
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;

		public MediaApiController(IMediaService mediaService, ILogger<MediaApiController> logger, IConfiguration configuration)
		{
			_mediaService = mediaService;
			_logger = logger;
			_configuration = configuration;
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
				_self = _configuration["Media:AccessPath"].EndsWith("/")
					? new Uri(_configuration["Media:AccessPath"] + f.Name)
					: new Uri(_configuration["Media:AccessPath"] + "/" + f.Name)
			});
		}


		[HttpDelete("{filename}")]
		public async Task<IActionResult> DeleteMedia([FromRoute] string filename)
		{
			try
			{
				await _mediaService.DeleteFileAsync(filename);
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
