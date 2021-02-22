using System;
using Lightning.Core.Generated;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace Lightning.Controller.ProjectEdits
{
	public class GrpcProjectEditService : Core.Generated.GrpcProjectEditService.GrpcProjectEditServiceBase
	{
		public GrpcProjectEditService()
		{

		}


		public override async Task GetEditChangeStream(GeneralRequest request,
			IServerStreamWriter<EditChangeMessage> responseStream,
			ServerCallContext context)
		{
			
		}


	}
}
