using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Projects.Structure
{
	public class NodeInfo
	{
		public string Id { get; set; } = string.Empty;
		public string Nodename { get; set; } = string.Empty;

		public int FramesPerSecond { get; set; }
		public int DisplayHeight { get; set; }
		public int DisplayWidth  { get; set; }

		public float TimerSpeedAdjustment { get; set; }
	}
}
