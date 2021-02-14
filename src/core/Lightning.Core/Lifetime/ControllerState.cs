using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public enum ControllerState : byte
	{
		/// <summary>
		/// Rendertree can be modified, output screen is blank.
		/// </summary>
		Ready = 1,
		/// <summary>
		/// Node is on air and shows content. Rendertree is fixed.
		/// </summary>
		Live = 2,
	}
}
