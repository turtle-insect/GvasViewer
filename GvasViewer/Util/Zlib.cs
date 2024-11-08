using System.IO;

namespace GvasViwer.Util
{
	internal class Zlib
	{
		public static Byte[] Compression(Byte[] buffer)
		{
			using var input = new MemoryStream(buffer);
			using var output = new MemoryStream();
			using var zlib = new System.IO.Compression.ZLibStream(output, System.IO.Compression.CompressionLevel.Fastest);
			input.CopyTo(zlib);

			return output.ToArray();
		}

		public static Byte[] Decompress(Byte[] buffer)
		{
			using var input = new MemoryStream(buffer);
			using var zlib = new System.IO.Compression.ZLibStream(input, System.IO.Compression.CompressionMode.Decompress);
			using var output = new MemoryStream();
			zlib.CopyTo(output);

			return output.ToArray();
		}
	}
}
