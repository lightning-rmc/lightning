using System;
using System.IO;

namespace Lightning.Core.Media
{
	public class Media
	{
		public string Name { get; set; } = string.Empty;
		public string Extension { get; set; } = string.Empty;
		public long Size { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime ModifiedOn { get; set; }
		public string? Hash { get; set; }


		public static Media FromFileInfo(FileInfo f)
			=> new Media()
			{
				Name = f.Name,
				Extension = f.Extension,
				CreatedOn = f.CreationTime,
				ModifiedOn = f.LastWriteTime,
				Hash = MediaUtil.ComputeHash(f.FullName),
				Size = f.Length
			};
	}
}
