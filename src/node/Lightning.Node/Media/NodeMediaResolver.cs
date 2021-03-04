using Lightning.Core.Media;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node.Media
{
	public class NodeMediaResolver : IMediaResolver
	{
		private readonly NodeConfiguration _options;

		public NodeMediaResolver(IOptions<NodeConfiguration> options)
		{
			_options = options.Value;
		}


		public string? GetFilePath(string filename)
		{
			//TODO: check if the file exists
			//TODO: add logging
			return Path.Combine(_options.Media.StoragePath, filename);
		}
	}
}
