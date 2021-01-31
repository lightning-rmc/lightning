using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Lightning.Node.Communications
{
	internal class ConfigurationConnectionResolver : IConnectionResolver
	{
		private readonly IConfiguration _configuration;

		public ConfigurationConnectionResolver(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public Task<ConnectionInfo> GetConnectionInfoAsync(string? identifier = null)
		{
			var conf = _configuration.GetSection("ServerConenction");
			return Task.FromResult<ConnectionInfo>(
				new(conf.GetValue<string>("Adress"), conf.GetValue<int>("Port"), identifier ?? string.Empty));
		}


	}
}
