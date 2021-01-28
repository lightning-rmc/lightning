using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Utils
{
	public static class CreateOnStartupServiceCollectionExtensions
	{

		public static IServiceCollection AddCreateOnStarup<TService>(this IServiceCollection services) where TService : class, ICreateOnStartup
		{
			if (services is null)
				throw new ArgumentNullException(nameof(services), string.Empty);
			services.TryAddEnumerable(new ServiceDescriptor(typeof(IHostedService),typeof(CreateOnStarupBootstrapper), ServiceLifetime.Singleton));
			services.AddSingleton<ICreateOnStartup, TService>();
			return services;
		}
	}
}
