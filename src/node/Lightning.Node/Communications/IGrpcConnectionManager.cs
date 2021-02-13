using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lightning.Core.Generated;

namespace Lightning.Node.Communications
{
	public interface IGrpcConnectionManager : IDisposable
	{
		bool ServerFound { get; }
		Task SearchAndAuthenticateForServerAsync(CancellationToken token = default);

		GrpcLayerEditService.GrpcLayerEditServiceClient GetLayerEditServiceClient();
		GrpcLifeTimeService.GrpcLifeTimeServiceClient GetLifetimeServiceClient();
		//GrpcMediaService.GrpcMediaServiceClient GetMediaServiceClient();
		GrpcTimeService.GrpcTimeServiceClient GetTimeServiceClient();

	}
}
