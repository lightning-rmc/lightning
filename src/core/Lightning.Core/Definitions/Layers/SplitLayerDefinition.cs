using Lightning.Core.Configuration;
using Lightning.Core.Definitions.Collections;
using Portable.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	[ContentProperty(nameof(Childs))]
	public class SplitLayerDefinition : LayerBaseDefinition
	{
		public SplitLayerDefinition()
		{
			_childs = new();
		}

		private LayerBaseDefinitionCollectionCollection _childs;
		public LayerBaseDefinitionCollectionCollection Childs { get => _childs; set => Set(ref _childs, value); }


		//TODO: handle Collections?
		protected override ConfigurationChangedTarget Type => throw new NotImplementedException();
	}
}
