using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core
{
	public class FeatureFlags
	{
		public bool DisableFullscreen { get; set; }
		public bool NodeWithoutServer { get; set; }
		public bool AddDummyNodeServices { get; set; }
	}
}
