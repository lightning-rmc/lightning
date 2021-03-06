using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lightning.Core.Generated;

namespace Lightning.Node.Communications
{
	public interface IConnectionManager : IDisposable
	{
		bool ServerFound { get; }

		//TODO: Remove if we have service over the httpclient
		public string NodeId { get; } 
		Task SearchAndAuthenticateForServerAsync(CancellationToken token = default);

		HttpClient GetHttpClient();
		GrpcProjectEditService.GrpcProjectEditServiceClient GetProjectEditServiceClient();
		GrpcLifetimeService.GrpcLifetimeServiceClient GetLifetimeServiceClient();
		GrpcMediaSyncService.GrpcMediaSyncServiceClient GetMediaServiceClient();
		GrpcTimeService.GrpcTimeServiceClient GetTimeServiceClient();

	}
}
