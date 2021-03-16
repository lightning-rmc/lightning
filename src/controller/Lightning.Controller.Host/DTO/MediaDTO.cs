using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.DTO
{
	public class MediaDTO : Core.Media.Media
	{
		public Uri _self { get; set; } = new Uri("");
	}
}
