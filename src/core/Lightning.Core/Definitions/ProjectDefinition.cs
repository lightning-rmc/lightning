
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
			_layerGroups = new();
		}

		private NodeDefinitionCollection _nodes;
		public NodeDefinitionCollection Nodes { get => _nodes; set => Set(ref _nodes, value); }

		private LayerGroupDefinitionCollection _layerGroups;
		public LayerGroupDefinitionCollection LayerGroups { get => _layerGroups; set => Set(ref _layerGroups, value); }

		protected override ConfigurationChangedTarget Type => throw new NotImplementedException();
	}
}
