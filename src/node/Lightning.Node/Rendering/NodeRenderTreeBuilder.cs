using Lightning.Core.Definitions;
using Lightning.Core.Definitions.Layers;
using Lightning.Core.Presentation;
using Lightning.Core.Rendering;
using Lightning.Core.Rendering.Layers;
using Lightning.Node.Communications;
using Microsoft.Extensions.DependencyInjection;
using OpenCvSharp;
using Portable.Xaml;
using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Node.Rendering
{

	//TODO: Move in Core abstract dependencies with interfaces
	public class NodeRenderTreeBuilder : IRenderTreeBuilder<Mat>
	{
		private readonly IConnectionManager _connectionManager;
		private readonly IServiceProvider _provider;

		public NodeRenderTreeBuilder(IConnectionManager connectionManager, IServiceProvider provider)
		{
			_connectionManager = connectionManager;
			_provider = provider;
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
				next = BuildLayer(definition, next);
				layerDic.Add(next.Name, next);
			}

			//TODO: Add property synchronization
			var grpcClient = _connectionManager.GetLifetimeServiceClient();
			var observer = new LayerActivationObserver<Mat>(layerDic, grpcClient);
			var obseravationWrapper = new ObseravtionWrapperLayer(observer, next);
			return obseravationWrapper;
		}


		protected virtual ILayer<Mat> BuildLayer(LayerBaseDefinition definition, ILayer<Mat>? next)
		{
			ILayer<Mat> layer = definition switch
			{
				LayerDefinition def => ActivatorUtilities.CreateInstance<OpenCVLayer>(_provider, def, next!),
				SplitLayerDefinition def => null!,
				WindowOutputDefinition def => ActivatorUtilities.CreateInstance<WindowHostOutputLayer>(_provider, def),
				_ => throw new NotImplementedException(),
			};
			//TODO: add logging

			return layer;
		}
	}
}
