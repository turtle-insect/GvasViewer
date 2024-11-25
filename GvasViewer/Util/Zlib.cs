using System.IO;

namespace GvasViewer.Util
{
	internal class Zlib
	{
		public static Byte[] Compression(Byte[] buffer)
		{
			Byte[] result = [];
			using (var input = new MemoryStream(buffer))
			{
				using (var output = new MemoryStream())
				{
					using (var zlib = new System.IO.Compression.ZLibStream(output, System.IO.Compression.CompressionLevel.Fastest))
					{
						input.CopyTo(zlib);
					}
					result = output.ToArray();
				}
			}

			return result;
		}

		public static Byte[] Decompress(Byte[] buffer)
		{
			Byte[] result = [];
			using (var input = new MemoryStream(buffer))
			{
				using (var zlib = new System.IO.Compression.ZLibStream(input, System.IO.Compression.CompressionMode.Decompress))
				{
					using (var output = new MemoryStream())
					{
						zlib.CopyTo(output);
						result = output.ToArray();
					}
				}
			}

			return result;
		}
	}
}
