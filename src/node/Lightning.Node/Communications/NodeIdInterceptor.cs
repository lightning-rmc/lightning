using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node.Communications
{
	public class NodeIdInterceptor : Interceptor
	{
		public readonly Metadata.Entry _nodeEntry;
		public NodeIdInterceptor(IOptions<NodeConfiguration> conf)
		{
			_nodeEntry = new Metadata.Entry("nodeId", conf.Value.NodeId);
		}

		public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
			IAsyncStreamReader<TRequest> requestStream,
			ServerCallContext context,
			ClientStreamingServerMethod<TRequest, TResponse> continuation)
		{
			context.RequestHeaders.Add(_nodeEntry);
			return base.ClientStreamingServerHandler(requestStream, context, continuation);
		}

		public override Task DuplexStreamingServerHandler<TRequest, TResponse>(
			IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream,
			ServerCallContext context,
			DuplexStreamingServerMethod<TRequest, TResponse> continuation)
		{
			context.RequestHeaders.Add(_nodeEntry);
			return base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
		}

		public override Task ServerStreamingServerHandler<TRequest, TResponse>(
			TRequest request,
			IServerStreamWriter<TResponse> responseStream,
			ServerCallContext context,
			ServerStreamingServerMethod<TRequest, TResponse> continuation)
		{
			context.RequestHeaders.Add(_nodeEntry);
			return base.ServerStreamingServerHandler(request, responseStream, context, continuation);
		}

		public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
			TRequest request,
			ServerCallContext context,
			UnaryServerMethod<TRequest, TResponse> continuation)
		{
			context.RequestHeaders.Add(_nodeEntry);
			return base.UnaryServerHandler(request, context, continuation);
		}
	}
}
