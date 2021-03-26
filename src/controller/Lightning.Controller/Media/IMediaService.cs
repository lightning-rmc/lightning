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
		IEnumerable<Core.Media.Media> GetFiles(bool ignoreCache);

		Task SaveFileAsync(IFormFile file);

		Task DeleteFileAsync(string filename);

		bool ExistsMedia(string filename);

		IAsyncEnumerable<(string fileName, UpdateType updateType)> GetUpdatesAllAsync();
	}
}
