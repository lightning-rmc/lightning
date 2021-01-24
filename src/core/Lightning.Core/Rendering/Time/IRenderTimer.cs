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
		void StartTimer();
		void StopTimer();
		IEnumerable<int> GetTimerStream();
	}
}
