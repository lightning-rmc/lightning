using Lightning.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public class ResolutionDefinition : DefinitionBaseType
	{
		public ResolutionDefinition()
		{

		}

		public ResolutionDefinition(int height, int width)
		{
			Height = height;
			Width = width;
		}

		private int _height;
		public int Height { get => _height; set => Set(ref _height, value); }

		private int _width;
		public int Width { get => _width; set => Set(ref _width, value); }

		public static ResolutionDefinition UHD => new(2160, 3840);
		public static ResolutionDefinition FullHD => new(1080, 1920);
		public static ResolutionDefinition HDReady => new(720, 1280);

		protected override ConfigurationChangedTarget Type => throw new NotImplementedException();
	}
}
