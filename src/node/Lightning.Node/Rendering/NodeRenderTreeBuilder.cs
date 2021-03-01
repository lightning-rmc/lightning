using Lightning.Core.Definitions;
using Lightning.Core.Definitions.Layers;
using Lightning.Core.Presentation;
using Lightning.Core.Rendering;
using Lightning.Core.Rendering.Layers;
using Lightning.Node.Communications;
using OpenCvSharp;
using Portable.Xaml;
using System;
using System.Collections.Generic;
using System.Threading.Channels;
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
			//TODO: Refactor httpclient it in service
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

		private ILayer<Mat> BuildTreeInternal(RenderTreeDefinition renderTreeDefinition)
		{
			var layerDic = new Dictionary<string, ILayer<Mat>>();
			var layerDefinitions = renderTreeDefinition.Layers;
			ILayer<Mat> next = null!;
			for (int i = layerDefinitions.Count - 1; i >= 0; i--)
			{
				var definition = layerDefinitions[i];
				next = BuildLayer(definition, next, layerDic);
			}

			var grpcClient = _connectionManager.GetLifetimeServiceClient();
			var observer = new LayerActivationObserver<Mat>(layerDic, grpcClient);
			var obseravationWrapper = new ObseravtionWrapperLayer(observer, next);
			//return next;
			//return new DummyLayer(_windowHost);
			return obseravationWrapper;
		}


		private ILayer<Mat> BuildLayer(LayerBaseDefinition definition, ILayer<Mat>? next, IDictionary<string, ILayer<Mat>> layers)
		{
			var layer = definition switch
			{
				LayerDefinition basicLayer => null!,
				SplitLayerDefinition splitLayer => null!,
				WindowOutputDefinition outputDefinition => LayerBuilder.BuildOutputWindowLayer(_windowHost, outputDefinition),
				_ => throw new NotImplementedException(),
			};


			return layer;
		}
	}
}
