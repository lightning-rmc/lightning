using Lightning.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public class NodeDefinition : DefinitionBaseType
	{
		public NodeDefinition()
		{
			_resolution = new();
			_name = string.Empty;
		}

		private string _name;
		public string Name { get => _name; set => Set(ref _name, value); }

		private int _framesPerSecond;
		public int FramesPerSecond { get => _framesPerSecond; set => Set(ref _framesPerSecond,value); }

		private ResolutionDefinition _resolution;
		public ResolutionDefinition Resolution { get=> _resolution; set => Set(ref _resolution,value); }

		protected override ConfigurationChangedTarget Type => throw new NotImplementedException();
	}
}
