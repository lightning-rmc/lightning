using Grpc.Core;
using Lightning.Core.Generated;
using Lightning.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Timer
{

	public class GrpcTimerService : Core.Generated.GrpcTimeService.GrpcTimeServiceBase
	{
		private readonly ITimerService _timerService;

		public GrpcTimerService(ITimerService timerService)
		{
			_timerService = timerService;
		}

		public override async Task GetSychronisationStream(RequestTimestampStream request, IServerStreamWriter<TimestampMessage> responseStream, ServerCallContext context)
		{
			try
			{
				await foreach (var tick in _timerService.GetTimerTicks(context.CancellationToken))
				{
					await responseStream.WriteAsync(new() { Tick = tick });
				}
			}
			catch (OperationCanceledException e)
			{
			}
		}
	}
}
