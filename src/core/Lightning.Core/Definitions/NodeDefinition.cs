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
			_id = string.Empty;
			_name = string.Empty;
		}


		private string _id;
		public string Id { get => _id; set => Set(ref _id, value); }

		private string _name;
		public string Name { get => _name; set => Set(ref _name, value); }

		private int _framesPerSecond;
		public int FramesPerSecond { get => _framesPerSecond; set => Set(ref _framesPerSecond,value); }

		private ResolutionDefinition _resolution;
		public ResolutionDefinition Resolution { get=> _resolution; set => Set(ref _resolution,value); }

	}
}
