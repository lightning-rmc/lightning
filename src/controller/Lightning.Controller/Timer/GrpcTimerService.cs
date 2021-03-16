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
		public GrpcTimerService()
		{

		}

		public override async Task GetSychronisationStream(RequestTimestampStream request, IServerStreamWriter<TimestampMessage> responseStream, ServerCallContext context)
		{

		}
	}
}
