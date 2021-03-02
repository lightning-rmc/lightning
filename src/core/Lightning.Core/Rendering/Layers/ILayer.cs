using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering
{
	public interface ILayer<in TFrame> : IDisposable
	{
		string Name { get; }

		bool IsActive { get; set; }

		void Process(TFrame frame, int tick);

	}
}
