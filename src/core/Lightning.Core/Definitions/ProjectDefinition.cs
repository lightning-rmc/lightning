
using Lightning.Core.Definitions.Collections;
using Portable.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;


[assembly: XmlnsDefinition("http://schemas.ligthing-rmc.com/project/definition", "Lightning.Core.Definitions")]
[assembly: XmlnsDefinition("http://schemas.ligthing-rmc.com/project/definition", "Lightning.Core.Definitions.Layers")]
[assembly: XmlnsDefinition("http://schemas.ligthing-rmc.com/project/definition", "Lightning.Core.Definitions.Collections")]
namespace Lightning.Core.Definitions
{
	public class ProjectDefinition
	{
		public ProjectDefinition()
		{
			Nodes = new();
			RenderTrees = new();
		}

		public NodeDefinitionCollection Nodes { get; set; }

		public RenderTreeDefinitionCollection RenderTrees { get; set; }


	}
}
