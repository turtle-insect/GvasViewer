using System.IO;

namespace GvasViewer.FileFormat.Util
{
	internal class Zlib
	{
		public static Byte[] Compression(Byte[] buffer)
		{
			using System.IO.MemoryStream output = new();
			using (System.IO.Compression.ZLibStream zlib = new(output, System.IO.Compression.CompressionLevel.Optimal))
			{
				zlib.Write(buffer);
			}

			return output.ToArray();
		}

		public static Byte[] Decompress(Byte[] buffer)
		{
			using System.IO.MemoryStream input = new(buffer);
			using System.IO.MemoryStream output = new();
			using (System.IO.Compression.ZLibStream zlib = new(input, System.IO.Compression.CompressionMode.Decompress))
			{
				zlib.CopyTo(output);
			}

			return output.ToArray();
		}
	}
}
