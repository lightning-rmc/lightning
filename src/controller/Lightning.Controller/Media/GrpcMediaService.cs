using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Lightning.Core.Generated;

namespace Lightning.Controller.Media
{
	public class GrpcMediaService : GrpcMediaSyncService.GrpcMediaSyncServiceBase
	{
		private readonly IMediaService _mediaService;

		public GrpcMediaService(IMediaService mediaService)
		{
			_mediaService = mediaService;
		}

		public async override Task GetMediaUpdates(MediaUpdateRequest request, IServerStreamWriter<MediaUpdateResponse> responseStream, ServerCallContext context)
		{
			await foreach (var update in _mediaService.GetUpdatesAllAsync())
			{
				await responseStream.WriteAsync(new()
				{
					FileName = update.fileName,
					UpdateType = update.updateType switch
					{
						UpdateType.ADDED => Core.Generated.UpdateType.Add,
						UpdateType.CHANGED => Core.Generated.UpdateType.Change,
						UpdateType.DELETED => Core.Generated.UpdateType.Delete,
						_ => throw new NotImplementedException()
					}
				});
			}
		}
	}
}
