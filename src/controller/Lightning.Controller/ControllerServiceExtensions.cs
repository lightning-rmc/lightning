using Lightning.Controller.Lifetime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller
{
	public static class ControllerServiceExtensions
	{
		public static IServiceCollection AddNodeLifetime(this IServiceCollection services)
		{
			services.TryAddSingleton<INodeLifetimeService, NodeLifetimeService>();
			return services;
		}

		public static IServiceCollection AddControllerServices(this IServiceCollection services)
		{
			services.AddNodeLifetime();
			return services;
		}
	}
}
