using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Core.Configuration
{
    public interface IConfigurationHandler<out TConfiguration> 
        : IConfigurationHandler where TConfiguration : class
    {
        TConfiguration GetConfiguration(string? name = null);
    }

    public interface IConfigurationHandler
    {
        Task ConnectAsync(CancellationToken token = default);
    }
}
