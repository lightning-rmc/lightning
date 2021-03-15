using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Controller.Projects
{
	public interface IProjectLoader
	{
		Task PersistProjectAsync(CancellationToken token = default);
	}
}