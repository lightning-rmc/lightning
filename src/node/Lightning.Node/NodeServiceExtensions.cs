using Lightning.Core;
using Lightning.Core.Utils;
using Lightning.Node.Communications;
using Lightning.Node.Lifetime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node
{
	public static class NodeServiceExtensions
	{
		public static IServiceCollection AddNodeCoreServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddRendering();
			services.AddOpenCVWindowHost();
			services.AddFeatureFlags(configuration);
			services.AddCreateOnStartup<NodeLifetimeController>();
			
			return services;
		}

		public static IServiceCollection AddGrpcRemoteServices(this IServiceCollection services)
		{
			services.AddSingleton<IGrpcConnectionManager, GrpcConnectionManager>();
			return services;
		}
	}
}
