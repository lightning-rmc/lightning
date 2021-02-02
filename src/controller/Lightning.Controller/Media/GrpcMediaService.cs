using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Lightning.Core.Generated;

namespace Lightning.Controller.Media
{
	class GrpcMediaService : MediaSyncService.MediaSyncServiceBase
	{
		private readonly IMediaService _mediaService;

		public GrpcMediaService(IMediaService mediaService)
		{
			_mediaService = mediaService;
		}

		public override Task GetMediaUpdates(MediaUpdateRequest request, IServerStreamWriter<MediaUpdateResponse> responseStream, ServerCallContext context)
		{
			return base.GetMediaUpdates(request, responseStream, context);
		}
	}
}
