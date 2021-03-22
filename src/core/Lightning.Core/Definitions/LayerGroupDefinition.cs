using Lightning.Core.Configuration;
using Lightning.Core.Definitions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public class LayerGroupDefinition : DefinitionBaseType
	{
		public LayerGroupDefinition()
		{
			_renderTree = new();
			_timerConfig = new();
		}

		private NodeLinkCollection	_nodes;

		public NodeLinkCollection Nodes
		{
			get => _nodes;
			set => Set(ref _nodes, value);
		}


		private TimerDefinition _timerConfig;
		public TimerDefinition TimerConfig
		{
			get => _timerConfig;
			set => Set(ref _timerConfig, value);
		}

		private RenderTreeDefinition _renderTree;
		public RenderTreeDefinition RenderTree { get => _renderTree; set => Set(ref _renderTree, value); }

		protected override ConfigurationChangedTarget Type => throw new NotImplementedException();
	}
}
