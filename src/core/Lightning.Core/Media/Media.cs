using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Media
{
	public record Media(string Name, string Extension, long Size, DateTime CreatedOn, DateTime LastWrite, string? Hash = null, Uri? _self = null);
}
