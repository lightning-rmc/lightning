using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public abstract class PropertyUpdate
	{
		public UpdateTarget Target { get; set; }
		public string Id { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;

	}

	public class PropertyUpdate<TValue> : PropertyUpdate where TValue : struct
	{
		public TValue Value { get; set; }
	}

	public enum UpdateTarget
	{
		Node,
		Layer,
		Transform,
		Color,
		Blendmode,

	}
}
