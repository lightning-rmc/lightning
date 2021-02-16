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

		
		public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
			TRequest request,
			ClientInterceptorContext<TRequest, TResponse> context,
			AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
		{
			AddMetadata(ref context);
			return base.AsyncServerStreamingCall(request, context, continuation);
		}

		public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
			TRequest request, ClientInterceptorContext<TRequest,
				TResponse> context,
			AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
		{
			AddMetadata(ref context);
			return base.AsyncUnaryCall(request, context, continuation);
		}

		public override TResponse BlockingUnaryCall<TRequest, TResponse>(
			TRequest request,
			ClientInterceptorContext<TRequest, TResponse> context,
			BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
		{
			AddMetadata(ref context);
			return base.BlockingUnaryCall(request, context, continuation);
		}

		public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
			ClientInterceptorContext<TRequest, TResponse> context,
			AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
		{
			AddMetadata(ref context);
			return base.AsyncClientStreamingCall(context, continuation);
		}

		public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
			ClientInterceptorContext<TRequest, TResponse> context,
			AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
		{
			AddMetadata(ref context);
			return base.AsyncDuplexStreamingCall(context, continuation);
		}


		private void AddMetadata<TRequest, TResponse>(ref ClientInterceptorContext<TRequest, TResponse> context)
		   where TRequest : class
		   where TResponse : class
		{
			var headers = context.Options.Headers;

			// Call doesn't have a headers collection to add to.
			// Need to create a new context with headers for the call.
			if (headers == null)
			{
				headers = new Metadata();
				var options = context.Options.WithHeaders(headers);
				context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);
			}

			// Add caller metadata to call headers
			headers.Add(_nodeEntry);
		}

	}
}
