using Grpc.Core;
using Lightning.Core.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Projects
{
	public class GrpcConfigurationService : GrpcProjectEditService.GrpcProjectEditServiceBase
	{

		public GrpcConfigurationService()
		{

		}

		public override Task GetEditChangeStream(GeneralRequest request,
			IServerStreamWriter<EditChangeMessage> responseStream,
			ServerCallContext context)
		{
			Console.WriteLine();
			return base.GetEditChangeStream(request, responseStream, context);
		}
	}
}
