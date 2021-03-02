using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Media
{
	public interface IMediaResolver
	{
		string? GetFilePath(string filename);
	}
}
