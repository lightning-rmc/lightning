
using Lightning.Core.Configuration;
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
	public class ProjectDefinition : DefinitionBaseType
	{
		public ProjectDefinition()
		{
			_nodes = new();
			_renderTrees = new();
		}

		private NodeDefinitionCollection _nodes;
		public NodeDefinitionCollection Nodes { get => _nodes; set => Set(ref _nodes, value); }

		private RenderTreeDefinitionCollection _renderTrees;
		public RenderTreeDefinitionCollection RenderTrees { get => _renderTrees; set => Set(ref _renderTrees, value); }

		protected override ConfigurationChangedTarget Type => throw new NotImplementedException();
	}
}
