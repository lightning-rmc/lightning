using Grpc.Core;
using Lightning.Core.Generated;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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
		private ServiceProvider? _serviceProvider;

		public ConnectionManager(IConnectionResolver connectionResolver, IConfiguration configuration)
		{
			_connectionResolver = connectionResolver;
			_configuration = configuration;
			_serviceProvider = null;
		}

		public bool ServerFound { get; private set; }

		public HttpClient GetHttpClient()
			=> _serviceProvider?.GetRequiredService<HttpClient>()
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
			//TODO: Add authentication
			//TODO: Get Id from configuration
			var nodeId = "Test2";
			var connectionInfo = await _connectionResolver.GetConnectionInfoAsync();
			ServerFound = true;
			//TODO: Maybe move https vs. http in configuration
			var baseUri = new Uri($@"https://{connectionInfo.IpAdress}:{connectionInfo.Port}");
			var httpClient = new HttpClient
			{
				BaseAddress = baseUri
			};
			////TODO: Handle Exception/repsonse
			var result = await httpClient.PostAsync("api/nodes/register/test", new StringContent(""));
			var content = await  result.Content.ReadAsStringAsync();
			var colllection = new ServiceCollection();
			colllection.AddNodeConfiguration(_configuration);
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
