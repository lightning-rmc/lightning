using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;

namespace Lightning.Core.Utils
{
	public static class CreateOnStartupServiceCollectionExtensions
	{

		public static IServiceCollection AddCreateOnStartup<TService>(this IServiceCollection services) where TService : class, ICreateOnStartup
		{
			if (services is null)
				throw new ArgumentNullException(nameof(services), string.Empty);
			services.TryAddEnumerable(new ServiceDescriptor(typeof(IHostedService), typeof(CreateOnStartupBootstrapper), ServiceLifetime.Singleton));
			services.AddSingleton<ICreateOnStartup, TService>();
			return services;
		}

		public static IServiceCollection AddCreateOnStartup<TService>(this IServiceCollection services, Func<IServiceProvider, TService> factory) where TService : class, ICreateOnStartup
		{
			if (services is null)
				throw new ArgumentNullException(nameof(services), string.Empty);
			services.TryAddEnumerable(new ServiceDescriptor(typeof(IHostedService), factory, ServiceLifetime.Singleton));
			services.AddSingleton<ICreateOnStartup, TService>();
			return services;
		}
	}
}
