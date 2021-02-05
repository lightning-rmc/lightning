using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public class ResolutionDefinition
	{
		public ResolutionDefinition()
		{

		}

		public ResolutionDefinition(int height, int length)
		{
			Height = height;
			Length = length;
		}

		public int Height { get; set; }
		public int Length { get; set; }

		public static ResolutionDefinition UHD => new ResolutionDefinition(2160, 3840);
		public static ResolutionDefinition FullHD => new ResolutionDefinition(1080, 1920);
		public static ResolutionDefinition HDReady =>  new ResolutionDefinition(720, 1280);
	}
}
