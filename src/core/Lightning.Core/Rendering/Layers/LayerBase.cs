using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering.Layers
{
	public abstract class LayerBase<TFrame> : ILayer<TFrame>
	{

		protected LayerBase(string name)
		{
			Name = name;
		}

		public string Name { get; }

		public virtual bool IsActive { get; set; }

		public void Process(TFrame frame, int tick)
		{
			if (CanProcess(frame, tick))
			{
				frame = InternalProcess(frame, tick);
			}
			ProcessChilds(frame, tick);
		}

		protected abstract TFrame InternalProcess(TFrame frame, int tick);
		protected abstract void ProcessChilds(TFrame frame,int tick);
		protected virtual bool CanProcess(TFrame frame, int tick) => IsActive;

		public virtual void Dispose()
		{
			//Note : No Disposing needed
		}
	}
}
