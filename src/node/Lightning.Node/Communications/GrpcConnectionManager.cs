using Lightning.Core.Generated;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Node.Communications
{
	internal class GrpcConnectionManager : IGrpcConnectionManager
	{
		private readonly IConnectionResolver _connectionResolver;
		private ServiceProvider? _serviceProvider;

		public GrpcConnectionManager(IConnectionResolver connectionResolver)
		{
			_connectionResolver = connectionResolver;
			_serviceProvider = null;
		}

		public bool ServerFound { get; private set; }



		public GrpcLayerEditService.GrpcLayerEditServiceClient GetLayerEditServiceClient()
			=> _serviceProvider?.GetRequiredService<GrpcLayerEditService.GrpcLayerEditServiceClient>()
				?? throw new InvalidOperationException($"First call  {nameof(SearchAndAuthenticateForServerAsync)} to get the Server Credentials");

		public GrpcLifeTimeService.GrpcLifeTimeServiceClient GetLifetimeServiceClient()
			=> _serviceProvider?.GetRequiredService<GrpcLifeTimeService.GrpcLifeTimeServiceClient>()
				?? throw new InvalidOperationException($"First call {nameof(SearchAndAuthenticateForServerAsync)} to get the Server Credentials");

		public GrpcMediaService.GrpcMediaServiceClient GetMediaServiceClient()
			=> _serviceProvider?.GetRequiredService<GrpcMediaService.GrpcMediaServiceClient>()
					   ?? throw new InvalidOperationException($"First call  {nameof(SearchAndAuthenticateForServerAsync)} to get the Server Credentials");

		public GrpcTimeService.GrpcTimeServiceClient GetTimeServiceClient()
			=> _serviceProvider?.GetRequiredService<GrpcTimeService.GrpcTimeServiceClient>()
				?? throw new InvalidOperationException($"First call  {nameof(SearchAndAuthenticateForServerAsync)} to get the Server Credentials");

		public async Task SearchAndAuthenticateForServerAsync(CancellationToken token = default)
		{
			//TODO: Add authentication

			var connectionInfo = await _connectionResolver.GetConnectionInfoAsync();
			ServerFound = true;
			//TODO: Maybe move https vs. http in configuration
			var baseUri = new Uri($@"https://{connectionInfo.IpAdress}:{connectionInfo.Port}");
			var colllection = new ServiceCollection();
			colllection.AddGrpcClient<GrpcLayerEditService.GrpcLayerEditServiceClient>(opt =>
			{
				opt.Address = baseUri;
			});
			colllection.AddGrpcClient<GrpcLifeTimeService.GrpcLifeTimeServiceClient>(opt =>
			{
				opt.Address = baseUri;
			});
			colllection.AddGrpcClient<GrpcMediaService.GrpcMediaServiceClient>(opt =>
			{
				opt.Address = baseUri;
			});
			colllection.AddGrpcClient<GrpcTimeService.GrpcTimeServiceClient>(opt =>
			{
				opt.Address = baseUri;
			});
			_serviceProvider = colllection.BuildServiceProvider();
		}

		public void Dispose()
		{
			_serviceProvider?.Dispose();
		}
	}
}
