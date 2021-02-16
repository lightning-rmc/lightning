using Grpc.Core;
using Grpc.Core.Interceptors;
using Lightning.Core.Generated;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Node.Communications
{
	internal class ConnectionManager : IConnectionManager
	{


		private string _httpClientName = "Controller";
		private readonly IConnectionResolver _connectionResolver;
		private readonly IConfiguration _configuration;
		private readonly NodeConfiguration _nodeConfiguration;
		private ServiceProvider? _serviceProvider;

		public ConnectionManager(IConnectionResolver connectionResolver, IConfiguration configuration, IOptions<NodeConfiguration> options)
		{
			_connectionResolver = connectionResolver;
			_configuration = configuration;
			_serviceProvider = null;
			_nodeConfiguration = options.Value;
		}

		public bool ServerFound { get; private set; }
		public string NodeId => _nodeConfiguration.NodeId;

		public HttpClient GetHttpClient()
			=> _serviceProvider?.GetRequiredService<IHttpClientFactory>().CreateClient(_httpClientName)
				?? throw new InvalidOperationException($"First call  {nameof(SearchAndAuthenticateForServerAsync)} to get the Server Credentials");

		public GrpcLayerEditService.GrpcLayerEditServiceClient GetLayerEditServiceClient()
			=> _serviceProvider?.GetRequiredService<GrpcLayerEditService.GrpcLayerEditServiceClient>()
				?? throw new InvalidOperationException($"First call  {nameof(SearchAndAuthenticateForServerAsync)} to get the Server Credentials");

		public GrpcLifeTimeService.GrpcLifeTimeServiceClient GetLifetimeServiceClient()
			=> _serviceProvider?.GetRequiredService<GrpcLifeTimeService.GrpcLifeTimeServiceClient>()
				?? throw new InvalidOperationException($"First call {nameof(SearchAndAuthenticateForServerAsync)} to get the Server Credentials");

		//public GrpcMediaService.GrpcMediaServiceClient GetMediaServiceClient()
		//	=> _serviceProvider?.GetRequiredService<GrpcMediaService.GrpcMediaServiceClient>()
		//			   ?? throw new InvalidOperationException($"First call  {nameof(SearchAndAuthenticateForServerAsync)} to get the Server Credentials");

		public GrpcTimeService.GrpcTimeServiceClient GetTimeServiceClient()
			=> _serviceProvider?.GetRequiredService<GrpcTimeService.GrpcTimeServiceClient>()
				?? throw new InvalidOperationException($"First call  {nameof(SearchAndAuthenticateForServerAsync)} to get the Server Credentials");

		public async Task SearchAndAuthenticateForServerAsync(CancellationToken token = default)
		{
			//TODO: Maybe split two operations into to Methods
			var nodeId = _nodeConfiguration.NodeId;
			var connectionInfo = await _connectionResolver.GetConnectionInfoAsync();
			ServerFound = true;

			//TODO: Maybe move https vs. http in configuration
			var baseUri = new Uri($@"https://{connectionInfo.IpAdress}:{connectionInfo.Port}");
			var httpClient = new HttpClient
			{
				BaseAddress = baseUri
			};
			////TODO: Handle Exceptions
			var result = await httpClient.PostAsync($"api/nodes/register/{nodeId}", new StringContent(""));
			//TODO: Handle Response object and make a guideline for api responses
			if (result.StatusCode is not (HttpStatusCode.NotModified or HttpStatusCode.OK))
			{
				throw new InvalidOperationException("Something went wrong while authenticate at the controller.");
			}
			//TODO: Log Message
			var content = await result.Content.ReadAsStringAsync();
			var nodeEntry = new Metadata.Entry("nodeId", _nodeConfiguration.NodeId);
			var colllection = new ServiceCollection();
			colllection.AddNodeConfiguration(_configuration);
			colllection.AddSingleton<NodeIdInterceptor>();
			colllection.AddGrpcClient<GrpcLayerEditService.GrpcLayerEditServiceClient>(opt =>
			{
				opt.Address = baseUri;
				

			}).AddInterceptor<NodeIdInterceptor>();
			colllection.AddGrpcClient<GrpcLifeTimeService.GrpcLifeTimeServiceClient>(opt =>
			{
				opt.Address = baseUri;
			}).AddInterceptor<NodeIdInterceptor>();
			//colllection.AddGrpcClient<GrpcMediaService.GrpcMediaServiceClient>(opt =>
			//{
			//	opt.Address = baseUri;
			//}).AddInterceptor<NodeIdInterceptor>();
			colllection.AddGrpcClient<GrpcTimeService.GrpcTimeServiceClient>(opt =>
			{
				opt.Address = baseUri;
			}).AddInterceptor<NodeIdInterceptor>();
			colllection.AddHttpClient(_httpClientName, client =>
			{
				client.BaseAddress = baseUri;

			});
			_serviceProvider = colllection.BuildServiceProvider();
		}

		public void Dispose()
		{
			_serviceProvider?.Dispose();
		}
	}
}