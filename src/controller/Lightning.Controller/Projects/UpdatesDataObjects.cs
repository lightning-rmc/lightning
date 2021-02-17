using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Projects
{
	public record LayerPropertyUpdate(string LayerId, string PropertyName, object value);

	public record RenderTreeStructureChanged(string RenderTreeId, string NewLayerId, int Position, string? SplitLayerId = null);


	//TODO: Discuss not sure?
	public record NodeChanged(string NodeId, NodeAction action, object? Value = null, string? PropertyName = null);

	public enum NodeAction
	{
		NodeAdd,
		NodeRemove,
		NodePropertyChanged
	}
}
