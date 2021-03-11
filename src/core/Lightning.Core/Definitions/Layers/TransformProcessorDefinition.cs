using Lightning.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public class TransformProcessorDefinition : DefinitionBaseType
	{
		private int _scaleX = 1;
		public int ScaleX
		{
			get { return _scaleX; }
			set { Set(ref _scaleX, value); }
		}

		private int _scaleY = 1;
		public int ScaleY
		{
			get { return _scaleY; }
			set { Set(ref _scaleY, value); }
		}

		private int _x;
		public int X
		{
			get { return _x; }
			set { Set(ref _x, value); }
		}

		private int _y;
		public int Y
		{
			get { return _y; }
			set { Set(ref _y, value); }
		}

		private int _rotation;

		public int Rotation
		{
			get { return _rotation; }
			set { Set(ref _rotation, value); }
		}


		protected override ConfigurationChangedTarget Type
			=> ConfigurationChangedTarget.Transform;
	}
}
