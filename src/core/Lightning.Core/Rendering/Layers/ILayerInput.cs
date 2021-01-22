using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering.Layers
{
	public interface ILayerInput
	{
		string Name { get; }

		Mat Process(int tick);
	}
}
