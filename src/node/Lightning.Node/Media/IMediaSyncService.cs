using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Node.Media
{
	public interface IMediaSyncService
	{
		Task DownloadMediaAsync(string fileName);
		Task SyncAllMediaAsync();
	}
}
