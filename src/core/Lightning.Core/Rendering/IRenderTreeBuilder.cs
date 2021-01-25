using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering
{
	public interface IRenderTreeBuilder<in TFrame>
	{
		ILayer<TFrame> BuildTree();
	}
}
