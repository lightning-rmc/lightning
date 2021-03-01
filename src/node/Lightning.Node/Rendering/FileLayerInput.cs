using Lightning.Core.Configuration;
using Lightning.Core.Definitions.Layers;
using Lightning.Core.Rendering.Layers;
using OpenCvSharp;
using System;
using System.IO;

namespace Lightning.Node.Rendering
{
	public class FileLayerInput : OpenCVLayerInputBase
	{
		private readonly FileInputLayerDefinition _definition;
		private VideoCapture _videoCapture;

		public FileLayerInput(MediaConfiguration configuration, FileInputLayerDefinition definition)
			: base(definition.Filename)
		{
			_definition = definition;
			_definition.ConfigurationChanged += Definition_ConfigurationChanged;
			_videoCapture = CreateVideoCapture(definition.Filename);
		}

		private VideoCapture CreateVideoCapture(string filename)
		{
			var basePath = "";
			return new VideoCapture(Path.Combine(basePath, filename));
		}

		private void Definition_ConfigurationChanged(object? sender, ConfigurationChangedEventArgs e)
		{
			//TODO: maybe add a new Base Valuechanged type.
			if (e.Context is ConfigurationValueChangedContext<string> context
				&& context.Name == nameof(_definition.Filename))
			{
				_videoCapture = CreateVideoCapture(_definition.Filename);
			}
		}

		public override Mat Process(int tick)
		{
			var image = new Mat<Vec3b>();
			_videoCapture.Read(image);
			if (image.Empty())
			{
				throw new NotImplementedException();
			}
			return image;
		}
	}
}
