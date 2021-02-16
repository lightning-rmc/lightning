using Lightning.Core.Definitions;
using Lightning.Core.Presentation;
using Lightning.Core.Rendering;
using Lightning.Core.Rendering.Layers;
using Lightning.Node.Communications;
using OpenCvSharp;
using Portable.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Node.Rendering
{
	internal class NodeRenderTreeBuilder : IRenderTreeBuilder<Mat>
	{
		private readonly IWindowHost<Mat> _windowHost;
		private readonly IConnectionManager _connectionManager;

		public NodeRenderTreeBuilder(IWindowHost<Mat> windowHost, IConnectionManager connectionManager)
		{
			_windowHost = windowHost;
			_connectionManager = connectionManager;
		}

		public async Task<ILayer<Mat>> BuildTreeAsync()
		{
			//TODO: Refctor it in service
			var httpClient = _connectionManager.GetHttpClient();
			var response = await httpClient.GetAsync($"api/rendering/{_connectionManager.NodeId}");
			//TODO: Handle response code...
			var stringResponse = await response.Content.ReadAsStringAsync();
			if (XamlServices.Parse(stringResponse) is not RenderTreeDefinition tree)
			{
				//TODO: add logging and meaningful exceptionmessage
				throw new InvalidOperationException();
			}
			return BuildTreeInternal(tree);
		}

		private ILayer<Mat> BuildTreeInternal(RenderTreeDefinition definition)
		{
			var layers = definition.Layers.Reverse<LayerBaseDefinition>().ToArray();
			for (int i = layers.Length - 1; i >= 0; i--)
			{
				var layer = layers[i];
				switch (layer)
				{
					case LayerDefinition basicLayer:
					case SplitLayerDefinition splitLayer:
					default:
						break;
				}
			}

			return new DummyLayer(_windowHost);
		}
	}
}
