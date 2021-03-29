using Lightning.Core.Definitions;
using Lightning.Core.Media;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using System;

namespace Lightning.Core.Rendering.Layers.Inputs
{
	public class FileLayerInput : OpenCVLayerInputBase
	{
		private readonly FileInputLayerDefinition _definition;
		private readonly IMediaResolver _mediaResolver;
		private readonly ILogger<FileLayerInput>? _logger;
		private VideoCapture? _videoCapture;
		private int _startTick = -1;
		private int _lastFrame = 0;

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
				_logger?.LogWarning("No matching media file could be found for the filename: '{filename}'. ", filename);
				return null;
			}
		}

		public override Mat Process(int tick)
		{
			if (_startTick == -1)
			{
				_startTick = tick;
			}
			var actualFrame = tick - _startTick;
			var delta = actualFrame - _lastFrame;
			_lastFrame = actualFrame;
			if (delta > 10)
			{
				_videoCapture?.Set(VideoCaptureProperties.PosFrames, actualFrame);
			}
			var image = new Mat<Vec3b>();
			if (_videoCapture?.Read(image) ?? false)
			{
				//TODO: Handle Looping
			}
			return image;
		}

		public override void Reset()
		{
			_videoCapture = CreateVideoCapture(_definition.Filename);
			_startTick = -1;
		}
	}
}
