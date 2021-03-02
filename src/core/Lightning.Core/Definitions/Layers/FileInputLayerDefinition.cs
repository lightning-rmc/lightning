using Lightning.Core.Configuration;

namespace Lightning.Core.Definitions.Layers
{
	public class FileInputLayerDefinition : InputLayerBaseDefinition
	{
		public FileInputLayerDefinition()
		{
			_filename = string.Empty;
		}


		private string _filename;

		public string Filename
		{
			get => _filename;
			set => Set(ref _filename, value);
		}


		protected override ConfigurationChangedTarget Type
			=> ConfigurationChangedTarget.Input;
	}
}
