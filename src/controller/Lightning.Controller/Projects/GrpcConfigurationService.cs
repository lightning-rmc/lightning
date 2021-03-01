using Grpc.Core;
using Lightning.Core.Configuration;
using Lightning.Core.Generated;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Lightning.Controller.Projects
{
	public class GrpcConfigurationService : GrpcProjectEditService.GrpcProjectEditServiceBase
	{
		private readonly IProjectManager _projectManager;
		private readonly ILogger<GrpcConfigurationService>? _logger;

		public GrpcConfigurationService(IProjectManager projectManager, ILogger<GrpcConfigurationService>? logger = null)
		{
			_projectManager = projectManager;
			_logger = logger;
		}

		public override async Task GetEditChangeStream(GeneralRequest request,
			IServerStreamWriter<ConfigurationChangedMessage> responseStream,
			ServerCallContext context)
		{
			await foreach (var config in _projectManager.GetConfigurationChangedAllAsync())
			{
				var message = new ConfigurationChangedMessage()
				{
					Id = config.Id,
					TargetType = (int)config.Target,
					
				};

				switch (config)
				{
					case ConfigurationValueChangedContext<int> value:
					{
						message.ValueChangedContext = new()
						{
							Integer = value.Value
						};
					}
					break;
					case ConfigurationValueChangedContext<float> value:
					{
						message.ValueChangedContext = new()
						{
							Float = value.Value
						};
					}
					break;
					case ConfigurationValueChangedContext<bool> value:
					{
						message.ValueChangedContext = new()
						{
							Boolean = value.Value
						};
					}
					break;
					case ConfigurationValueChangedContext<string> value:
					{
						message.ValueChangedContext = new()
						{
							String = value.Value
						};
					}
					break;
					default:
					{
						//TODO: add log message
					}
					break;
				}

				await responseStream.WriteAsync(message);
			}
		}
	}
}
