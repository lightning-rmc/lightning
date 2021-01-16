using Lightning.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering
{
    public static class ServiceCollectionRenderExtensions
    {

        public static IServiceCollection AddRendering(this IServiceCollection services)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IConfigurationHandler<RenderConfiguration>, GrpcRenderConfigurationHandler>();
            
            // TODO: Make it transient or move it in RenderFactory or something similar.
            //       To get the possibility for multi instances of a IRenderHost.
            services.TryAddSingleton<IRenderHost, RenderHost>();
            return services;
        }
    }
}
