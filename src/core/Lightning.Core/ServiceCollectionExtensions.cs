using Lightning.Core.Configuration;
using Lightning.Core.Rendering;
using Lightning.Core.Rendering.Time;
using Lightning.Core.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core
{
	public static class ServiceCollectionExtensions
	{

		public static IServiceCollection AddRenderingCore(this IServiceCollection services)
		{
			if (services is null)
				throw new ArgumentNullException(nameof(services));

			services.TryAddSingleton<IConfigurationHandler<RenderConfiguration>, GrpcRenderConfigurationHandler>();

			//TODO:  Make it transient or move it in RenderFactory or something similar.
			//       To get the possibility for multi instances of a IRenderHost.
			services.TryAddSingleton<IRenderHost, OpenCVRenderHost>();
			services.AddGrpcTimer();
			return services;
		}

		public static IServiceCollection AddGrpcTimer(this IServiceCollection services)
		{
			if (services is null)
				throw new ArgumentNullException(nameof(services));

			services.TryAddSingleton<IRenderTimer, RenderTimer>();
			return services;
		}



		public static IServiceCollection AddOpenCVWindowHost(this IServiceCollection services)
		{
			if (services.Any(s => s.ServiceType == typeof(IWindowHost<Mat>)))
			{
				services.TryAddSingleton<IWindowHost>(sp => sp.GetRequiredService<IWindowHost<Mat>>());
			}
			return services;
		}


		public static IServiceCollection AddFeatureFlags(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<FeatureFlags>(configuration.GetSection("FeatureFlags"));
			return services;
		}
	}
}
