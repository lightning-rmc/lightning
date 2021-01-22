using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering.Layers
{
	public class BasicLayer : LayerBase
	{
		private readonly ILayer _child;

		public BasicLayer(string name,ILayer child)
			: base(name)
		{
			_child = child;
		}

		protected override Mat InternalProcess(Mat frame,int tick)
		{
			return frame;
		}

		protected override void ProcessChilds(Mat frame, int tick)
		{
			_child.Process(frame, tick);
		}
	}
}
