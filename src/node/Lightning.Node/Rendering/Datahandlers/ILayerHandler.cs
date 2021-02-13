using Lightning.Core.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node.Rendering.Datahandlers
{
	public interface ILayerHandler<TDefinition>
	{
		TDefinition GetLayerDefinition();
	}
}
