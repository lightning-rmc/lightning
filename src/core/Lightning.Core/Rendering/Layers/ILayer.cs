using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering.Layers
{
	public interface ILayer
	{
		string Name { get; }

		bool IsActive { get; }

		void Process(Mat frame, int tick);

	}
}
