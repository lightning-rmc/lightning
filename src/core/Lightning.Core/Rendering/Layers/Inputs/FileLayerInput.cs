using Lightning.Core.Configuration;
using Lightning.Core.Definitions.Layers;
using Lightning.Core.Media;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using System;
using System.IO;

namespace Lightning.Core.Rendering.Layers.Inputs
{
	public class FileLayerInput : OpenCVLayerInputBase
	{
		private readonly FileInputLayerDefinition _definition;
		private readonly IMediaResolver _mediaResolver;
		private readonly ILogger<FileLayerInput>? _logger;
		private VideoCapture? _videoCapture;

		public FileLayerInput(FileInputLayerDefinition definition, IMediaResolver mediaResolver, ILogger<FileLayerInput>? logger)
			: base(definition.Filename)
		{
			_definition = definition;
			_mediaResolver = mediaResolver;
			_logger = logger;
			_videoCapture = CreateVideoCapture(definition.Filename);
			_definition.PropertyChanged += Definition_PropertyChanged;
		}

		private void Definition_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(_definition.Filename))
			{
				_videoCapture = CreateVideoCapture(_definition.Filename);
			}
		}

		private VideoCapture? CreateVideoCapture(string filename)
		{
			if (_mediaResolver.GetFilePath(filename) is string filepath)
			{
				return new VideoCapture(filepath);
			}
			else
			{
				//TODO: add logging
				return null;
			}
		}

		public override Mat Process(int tick)
		{
			var image = new Mat<Vec3b>();
			_videoCapture?.Read(image);
			if (image.Empty())
			{
				//TODO: Add logging
			}
			return image;
		}

		public override void Reset()
		{
			_videoCapture = CreateVideoCapture(_definition.Filename);
		}
	}
}
