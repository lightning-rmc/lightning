using System.Threading.Tasks;

namespace Lightning.Node.Communications
{
	public interface IConnectionResolver
	{
		Task<ConnectionInfo> GetConnectionInfoAsync(string? identifier = null);
	}
}

public record ConnectionInfo(string IpAdress, int Port, string Identifier);
