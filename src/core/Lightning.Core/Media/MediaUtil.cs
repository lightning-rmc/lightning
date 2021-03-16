using System;
using System.IO;
using System.Security.Cryptography;

namespace Lightning.Core.Media
{
	public class MediaUtil
	{
		public static string ComputeHash(string filePath)
		{
			using var algorithm = SHA256.Create();
			using var stream = File.OpenRead(filePath);
			var hash = algorithm.ComputeHash(stream);
			return BitConverter.ToString(hash);
		}
	}
}
