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
		IEnumerable<Media> GetFiles();

		Task SaveFileAsync(IFormFile file);
	}
}
