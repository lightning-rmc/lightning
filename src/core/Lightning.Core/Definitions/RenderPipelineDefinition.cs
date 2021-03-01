using Lightning.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public class RenderPipelineDefinition : DefinitionBaseType
	{
		public RenderPipelineDefinition()
		{
			_renderTree = new();
		}

		private RenderTreeDefinition _renderTree;
		public RenderTreeDefinition RenderTree { get => _renderTree; set => Set(ref _renderTree, value); }

		protected override ConfigurationChangedTarget Type => throw new NotImplementedException();
	}
}
