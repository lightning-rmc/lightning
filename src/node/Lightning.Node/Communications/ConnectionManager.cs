using Grpc.Core;
using Lightning.Core.Generated;
using Lightning.Core.Lifetime;
using Lightning.Node.Lifetime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Node.Communications
{
	internal class ConnectionManager : IConnectionManager
	{
		private readonly string _httpClientName = "Controller";
		private readonly IConnectionResolver _connectionResolver;
		private readonly IConfiguration _configuration;
		private readonly INodeStateNotifier _nodeLifetimeNotifier;
		private readonly ILogger<ConnectionManager>? _logger;
		private readonly NodeConfiguration _nodeConfiguration;
		private ServiceProvider? _serviceProvider;

		public ConnectionManager(IConnectionResolver connectionResolver,
			IConfiguration configuration,
			IOptions<NodeConfiguration> options,
			INodeStateNotifier nodeLifetimeNotifier,
			ILogger<ConnectionManager>? logger = null)
		{
			_serviceProvider = null;
			_connectionResolver = connectionResolver;
			_configuration = configuration;
			_nodeLifetimeNotifier = nodeLifetimeNotifier;
			_nodeConfiguration = options.Value;
			_logger = logger;
			_nodeLifetimeNotifier.StateChangeRequested += (s, e) =>
			{
				if (e.State == NodeState.Connected)
				{
					e.AddTask("Connect to Server",SearchAndAuthenticateForServerAsync(e.Token));
				}
			};
		}

		public bool ServerFound { get; private set; }
		public string NodeId => _nodeConfiguration.NodeId;

		public HttpClient GetHttpClient()
			=> _serviceProvider?.GetRequiredService<IHttpClientFactory>().CreateClient(_httpClientName)
				?? throw new InvalidOperationException($"First call  {nameof(SearchAndAuthenticateForServerAsync)} to get the Server Credentials");

		public GrpcProjectEditService.GrpcProjectEditServiceClient GetProjectEditServiceClient()
			=> _serviceProvider?.GetRequiredService<GrpcProjectEditService.GrpcProjectEditServiceClient>()
				?? throw new InvalidOperationException($"First call  {nameof(SearchAndAuthenticateForServerAsync)} to get the Server Credentials");

		public GrpcLifetimeService.GrpcLifetimeServiceClient GetLifetimeServiceClient()
			=> _serviceProvider?.GetRequiredService<GrpcLifetimeService.GrpcLifetimeServiceClient>()
				?? throw new InvalidOperationException($"First call {nameof(SearchAndAuthenticateForServerAsync)} to get the Server Credentials");

		public GrpcMediaSyncService.GrpcMediaSyncServiceClient GetMediaServiceClient()
			=> _serviceProvider?.GetRequiredService<GrpcMediaSyncService.GrpcMediaSyncServiceClient>()
					   ?? throw new InvalidOperationException($"First call  {nameof(SearchAndAuthenticateForServerAsync)} to get the Server Credentials");

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

			var nodeEntry = new Metadata.Entry("nodeId", _nodeConfiguration.NodeId);
			var collection = new ServiceCollection();
			collection.AddNodeConfiguration(_configuration);
			collection.AddSingleton<NodeIdInterceptor>();
			collection.AddGrpcClient<GrpcProjectEditService.GrpcProjectEditServiceClient>(opt =>
			{
				opt.Address = baseUri;
			}).AddInterceptor<NodeIdInterceptor>()
			.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			});
			collection.AddGrpcClient<GrpcLifetimeService.GrpcLifetimeServiceClient>(opt =>
			{
				opt.Address = baseUri;
			}).AddInterceptor<NodeIdInterceptor>()
			.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			});
			collection.AddGrpcClient<GrpcMediaSyncService.GrpcMediaSyncServiceClient>(opt =>
			{
				opt.Address = baseUri;
			}).AddInterceptor<NodeIdInterceptor>()
			.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			});
			collection.AddGrpcClient<GrpcTimeService.GrpcTimeServiceClient>(opt =>
			{
				opt.Address = baseUri;
			}).AddInterceptor<NodeIdInterceptor>()
			.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			});
			collection.AddHttpClient(_httpClientName, client =>
			{
				client.BaseAddress = baseUri;
			});
			//}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
			//{
			//	ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			//});
			_serviceProvider = collection.BuildServiceProvider();

			//TODO: Handle Response
			var r = GetLifetimeServiceClient().ConnectAsync(new());
			await r.ResponseAsync.ContinueWith(t =>
			{
				_logger?.LogInformation(t.Exception?.ToString());
				_logger?.LogInformation(t.Exception?.InnerException?.ToString());
				_logger?.LogInformation($"is faulted: {t.IsFaulted}");
			});
		}

		public void Dispose()
		{
			_serviceProvider?.Dispose();
		}
	}
}
