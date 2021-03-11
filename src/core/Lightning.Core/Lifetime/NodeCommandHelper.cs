using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	public static class NodeCommandHelper
	{
		public static NodeCommandResponse ConvertRequestoResponse(NodeCommandRequest request)
		{
			return request switch
			{
				NodeCommandRequest.GoLive => NodeCommandResponse.IsLive,
				NodeCommandRequest.GoReady => NodeCommandResponse.IsReady,
				NodeCommandRequest.HideInfo => NodeCommandResponse.HidingInfo,
				NodeCommandRequest.ShowInfo => NodeCommandResponse.ShowingInfo,
				NodeCommandRequest.TryConnecting => NodeCommandResponse.IsConnected,
				NodeCommandRequest.OnNodeStarted => NodeCommandResponse.NodeIsStarted,
				_ => throw new InvalidOperationException($"command {request} has no related response!")
			};
		}
	}
}
