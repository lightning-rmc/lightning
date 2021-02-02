using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Media
{
	public interface IMediaService
	{
		IEnumerable<Media> GetFiles(bool ignoreCache);

		Task SaveFileAsync(IFormFile file);

		IAsyncEnumerable<(string fileName, UpdateType updateType)> GetUpdates();
	}
}
