using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering
{
	public interface ILayerInput<out TFrame>
	{
		string Name { get; }

		TFrame Process(int tick);

		void Reset();
	}
}
