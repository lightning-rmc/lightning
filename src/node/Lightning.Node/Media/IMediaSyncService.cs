using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Node.Media
{
	public interface IMediaSyncService
	{
		IEnumerable<Core.Media.Media> GetLocalFiles();
		bool TryGetLocalFileByFilename(string filename, out Core.Media.Media? file);
		bool TryDeleteLocalFile(string filename);
		Task AddOrUpdateFileAsync(string filename, CancellationToken cancellationToken = default);
	}
}
