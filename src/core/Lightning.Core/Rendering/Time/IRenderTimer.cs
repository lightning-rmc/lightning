using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering.Time
{
	public interface IRenderTimer
	{
		bool IsRunning { get; }
		Task StartTimerAsync();
		void StopTimer();
		IEnumerable<int> GetTimerTicks();
	}
}
