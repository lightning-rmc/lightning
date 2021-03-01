using Lightning.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public class MediaDefinition : DefinitionBaseType
	{

		public MediaDefinition()
		{
			_name = string.Empty;
			_fileExtension = string.Empty;
			_hash = string.Empty;
		}


		private string _name;
		public string Name { get => _name; set => Set(ref _name, value); }
		private string _fileExtension;
		public string FileExtension { get => _fileExtension; set => Set(ref _fileExtension,value); }

		private string _hash;
		public string Hash { get=> _hash; set => Set(ref _hash,value); }

		protected override ConfigurationChangedTarget Type => throw new NotImplementedException();
	}
}
