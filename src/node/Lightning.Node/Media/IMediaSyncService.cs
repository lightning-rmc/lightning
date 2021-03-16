using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lightning.Node.Media
{
	public interface IMediaSyncService
	{
		IEnumerable<Core.Media.Media> GetLocalMedia();
		bool TryGetLocalMedia(string filename, out Core.Media.Media? file);
		Task SyncMediaFromController();
	}
}
