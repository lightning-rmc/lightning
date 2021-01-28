using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public enum NodeState : byte
	{

		/// <summary>
		/// Node shows debug information on output screen.
		/// </summary>
		Info = 0,
		/// <summary>
		/// Rendertree can be modified, output screen is blank.
		/// </summary>
		Edit = 1,
		/// <summary>
		/// Node is on air and shows content. Rendertree is fixed.
		/// </summary>
		Live = 2,
		/// <summary>
		/// Node is disconnected from controller.
		/// </summary>
		Offline = 3,


		/// <summary>
		/// Not in use Today.
		/// </summary>
		Error = byte.MaxValue
	}
}
