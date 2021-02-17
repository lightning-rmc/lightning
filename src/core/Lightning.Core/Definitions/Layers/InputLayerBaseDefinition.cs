using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public abstract class InputLayerBaseDefinition : DefinitionBaseType
	{
		protected InputLayerBaseDefinition()
		{

			_id = string.Empty;
		}

		private string _id;
		public string Id { get => _id; set => Set(ref _id, value); }
	}
}
