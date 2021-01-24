using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node.Communications
{
	public class ConfigurationConnectionResolver : IConnectionResolver
	{
		public Task<ConnectionInfo> GetConnectionInfoAsync(string? identifier = null)
		{
			throw new NotImplementedException();
		}
	}
}
