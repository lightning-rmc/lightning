using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering.Layers
{
	public abstract class OpenCVLayerInputBase : ILayerInput<Mat>
	{
		protected OpenCVLayerInputBase(string name)
		{
			Name = name;
		}

		public string Name { get; }

		public virtual Mat Process(int tick)
		{
			return Mat.Zeros(0, 0, MatType.CV_16SC3);
		}

		public abstract void Reset();

	}
}
