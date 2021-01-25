using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering.Layers
{
	public abstract class OpenCVLayerBase : ILayer<Mat>
	{

		protected OpenCVLayerBase(string name)
		{
			Name = name;
		}

		public string Name { get; }

		public virtual bool IsActive { get; protected set; }

		public void Process(Mat frame, int tick)
		{
			if (CanProcess(frame, tick))
			{
				frame = InternalProcess(frame, tick);
			}
			ProcessChilds(frame, tick);
		}

		protected abstract Mat InternalProcess(Mat frame, int tick);
		protected abstract void ProcessChilds(Mat frame,int tick);
		protected virtual bool CanProcess(Mat frame, int tick) => IsActive;
	}
}
