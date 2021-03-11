using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Configuration
{
	public enum ConfigurationChangedTarget
	{
		Layer = 1,
		Configuration = 2,

		Transform = 3,
		ColorCorrection = 4,
		BlendMode = 5,
		Input = 6,
		Output = 7,

		Unkown = int.MaxValue,
	}
}
